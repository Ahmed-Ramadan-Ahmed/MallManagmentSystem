using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly MallDBContext _context;
        private readonly IMessagingService _whatsAppService;
        private readonly IMessagingService _smsService;
        private readonly IConfiguration _configuration;

        public NotificationService(
            MallDBContext context,
            [FromKeyedServices("WhatsApp")] IMessagingService whatsAppService,
            [FromKeyedServices("SMS")] IMessagingService smsService,
            IConfiguration configuration)
        {
            _context = context;
            _whatsAppService = whatsAppService;
            _smsService = smsService;
            _configuration = configuration;
        }

        private bool ShouldSendWhatsApp(string severity)
        {
            return _configuration.GetValue<bool>($"Notifications:SeverityLevels:{severity}:SendWhatsApp", false);
        }

        private bool ShouldSendSMS(string severity)
        {
            return _configuration.GetValue<bool>($"Notifications:SeverityLevels:{severity}:SendSMS", false);
        }

        private bool ShouldNotifyAdmin(string severity)
        {
            return _configuration.GetValue<bool>($"Notifications:SeverityLevels:{severity}:NotifyAdmin", false);
        }

        private int GetRetentionDays(string severity)
        {
            return _configuration.GetValue<int>($"Notifications:RetentionDays:{severity}", 30);
        }

        public async Task CheckEmployeeContractExpiryAsync()
        {
            try
            {
                var warningDays = _configuration.GetValue<int>("Notifications:ContractExpiryWarningDays", 30);
                var expiryDate = DateTime.UtcNow.AddDays(warningDays);

                var expiringContracts = await _context.EmploymentContracts
                    .Include(c => c.Employee)
                    .Where(c => c.EndDate <= expiryDate && c.Status == "Active")
                    .ToListAsync();

                foreach (var contract in expiringContracts)
                {
                    var daysUntilExpiry = (contract.EndDate - DateTime.UtcNow).Days;
                    var severity = daysUntilExpiry <= 7 ? "Critical" : "Warning";

                    var notification = new Notification
                    {
                        Type = "ContractExpiry",
                        Title = "Employee Contract Expiring",
                        Message = $"Contract for employee {contract.Employee.Name} will expire in {daysUntilExpiry} days.",
                        CreatedAt = DateTime.UtcNow,
                        Severity = severity,
                        RecipientType = "Employee",
                        RecipientId = contract.EmployeeId,
                        RelatedEntityType = "EmploymentContract",
                        RelatedEntityId = contract.Id
                    };

                    await CreateNotificationAsync(notification);

                    if (ShouldSendWhatsApp(severity))
                    {
                        await SendContractExpiryNotificationAsync(contract.Employee.Phone, contract.Employee.Name, daysUntilExpiry);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to check employee contract expiry", ex);
            }
        }

        public async Task CheckStoreContractExpiryAsync()
        {
            try
            {
                var warningDays = _configuration.GetValue<int>("Notifications:ContractExpiryWarningDays", 30);
                var expiryDate = DateTime.UtcNow.AddDays(warningDays);

                var expiringContracts = await _context.StoreRentContracts
                    .Include(c => c.Store)
                    .Include(c => c.Renter)
                    .Where(c => c.EndDate <= expiryDate && c.Status == "Active")
                    .ToListAsync();

                foreach (var contract in expiringContracts)
                {
                    var daysUntilExpiry = (contract.EndDate - DateTime.UtcNow).Days;
                    var severity = daysUntilExpiry <= 7 ? "Critical" : "Warning";

                    var notification = new Notification
                    {
                        Type = "ContractExpiry",
                        Title = "Store Contract Expiring",
                        Message = $"Contract for store {contract.Store.Name} will expire in {daysUntilExpiry} days.",
                        CreatedAt = DateTime.UtcNow,
                        Severity = severity,
                        RecipientType = "Renter",
                        RecipientId = contract.RenterId,
                        RelatedEntityType = "StoreRentContract",
                        RelatedEntityId = contract.Id
                    };

                    await CreateNotificationAsync(notification);

                    if (ShouldSendWhatsApp(severity))
                    {
                        await SendContractExpiryNotificationAsync(contract.Renter.PhoneNumber, contract.Store.Name, daysUntilExpiry);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to check store contract expiry", ex);
            }
        }

        public async Task CheckRenterPaymentOverdueAsync()
        {
            try
            {
                var overdueDays = _configuration.GetValue<int>("Notifications:PaymentOverdueDays", 7);
                var dueDate = DateTime.UtcNow.AddDays(-overdueDays);

                var overduePayments = await _context.RentInvoices
                    .Include(p => p.Renter)
                    .Include(p => p.Store)
                    .Where(p => p.DueDate <= dueDate && p.IsPending)
                    .ToListAsync();

                foreach (var payment in overduePayments)
                {
                    var daysOverdue = (DateTime.UtcNow - payment.DueDate).Days;
                    var severity = daysOverdue >= 30 ? "Critical" : "Warning";

                    var notification = new Notification
                    {
                        Type = "PaymentOverdue",
                        Title = "Payment Overdue",
                        Message = $"Payment of {payment.RemainAmount} for store {payment.Store.Name} is overdue by {daysOverdue} days.",
                        CreatedAt = DateTime.UtcNow,
                        Severity = severity,
                        RecipientType = "Renter",
                        RecipientId = payment.RenterId,
                        RelatedEntityType = "RenterPayment",
                        RelatedEntityId = payment.Id
                    };

                    await CreateNotificationAsync(notification);

                    if (ShouldSendWhatsApp(severity))
                    {
                        await SendPaymentOverdueNotificationAsync(payment.Renter.PhoneNumber, payment.Store.Name, payment.RemainAmount, daysOverdue);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to check renter payment overdue", ex);
            }
        }

        public async Task CheckWorkerAbsenceAsync()
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var absentEmployees = await _context.Employees
                    .Where(e => e.IsActive == true)
                    .Where(e => !_context.Attendances
                        .Any(a => a.EmployeeId == e.Id && a.Date == today))
                    .ToListAsync();

                foreach (var employee in absentEmployees)
                {
                    var notification = new Notification
                    {
                        Type = "Absence",
                        Title = "Employee Absent",
                        Message = $"Employee {employee.Name} was absent today.",
                        CreatedAt = DateTime.UtcNow,
                        Severity = "Warning",
                        RecipientType = "Employee",
                        RecipientId = employee.Id,
                        RelatedEntityType = "Employee",
                        RelatedEntityId = employee.Id
                    };

                    await CreateNotificationAsync(notification);

                    if (ShouldSendWhatsApp("Warning"))
                    {
                        await SendAbsenceNotificationAsync(employee.Phone, employee.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to check worker absence", ex);
            }
        }

        public async Task CheckWorkerAbsenceLimitAsync()
        {
            try
            {
                var warningThreshold = _configuration.GetValue<int>("Notifications:AbsenceWarningThreshold", 3);
                var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

                var employeesExceedingLimit = await _context.Employees
                    .Where(e => e.IsActive == true)
                    .Select(e => new
                    {
                        Employee = e,
                        AbsenceCount = _context.Attendances
                            .Count(a => a.EmployeeId == e.Id && 
                                      a.Date >= monthStart && 
                                      a.Status == "Absent")
                    })
                    .Where(x => x.AbsenceCount >= warningThreshold)
                    .ToListAsync();

                foreach (var item in employeesExceedingLimit)
                {
                    var notification = new Notification
                    {
                        Type = "AbsenceLimit",
                        Title = "Absence Limit Exceeded",
                        Message = $"Employee {item.Employee.Name} has been absent {item.AbsenceCount} times this month.",
                        CreatedAt = DateTime.UtcNow,
                        Severity = "Critical",
                        RecipientType = "Employee",
                        RecipientId = item.Employee.Id,
                        RelatedEntityType = "Employee",
                        RelatedEntityId = item.Employee.Id
                    };

                    await CreateNotificationAsync(notification);

                    if (ShouldSendWhatsApp("Critical"))
                    {
                        await SendAbsenceLimitNotificationAsync(item.Employee.Phone, item.Employee.Name, item.AbsenceCount);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to check worker absence limit", ex);
            }
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            try
            {
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                return notification;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to create notification", ex);
            }
        }

        public async Task<List<Notification>> GetNotificationsByRecipientAsync(string recipientType, int recipientId)
        {
            try
            {
                return await _context.Notifications
                    .Where(n => n.RecipientType == recipientType && n.RecipientId == recipientId)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to get notifications by recipient", ex);
            }
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(string recipientType, int recipientId)
        {
            try
            {
                return await _context.Notifications
                    .Where(n => n.RecipientType == recipientType && 
                               n.RecipientId == recipientId && 
                               !n.IsRead)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to get unread notifications", ex);
            }
        }

        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                    notification.ReadAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to mark notification as read", ex);
            }
        }

        public async Task MarkAllNotificationsAsReadAsync(string recipientType, int recipientId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.RecipientType == recipientType && 
                               n.RecipientId == recipientId && 
                               !n.IsRead)
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                    notification.ReadAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to mark all notifications as read", ex);
            }
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification != null)
                {
                    _context.Notifications.Remove(notification);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to delete notification", ex);
            }
        }

        public async Task<int> GetUnreadNotificationCountAsync(string recipientType, int recipientId)
        {
            try
            {
                return await _context.Notifications
                    .CountAsync(n => n.RecipientType == recipientType && 
                                    n.RecipientId == recipientId && 
                                    !n.IsRead);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to get unread notification count", ex);
            }
        }

        private async Task SendContractExpiryNotificationAsync(string phoneNumber, string entityName, int daysUntilExpiry)
        {
            try
            {
                var message = $"IMPORTANT: Your contract for {entityName} will expire in {daysUntilExpiry} days. Please contact the administration.";
                
                if (_configuration.GetValue<bool>("Notifications:WhatsApp:Enabled", true))
                {
                    await _whatsAppService.SendWhatsAppMessageAsync(phoneNumber, message);
                }
                
                if (_configuration.GetValue<bool>("Notifications:SMS:Enabled", true))
                {
                    await _smsService.SendSMSAsync(phoneNumber, message);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to send contract expiry notification", ex);
            }
        }

        private async Task SendPaymentOverdueNotificationAsync(string phoneNumber, string storeName, decimal amount, int daysOverdue)
        {
            try
            {
                var message = $"URGENT: Payment of {amount} for store {storeName} is overdue by {daysOverdue} days. Please make the payment immediately.";
                
                if (_configuration.GetValue<bool>("Notifications:WhatsApp:Enabled", true))
                {
                    await _whatsAppService.SendWhatsAppMessageAsync(phoneNumber, message);
                }
                
                if (_configuration.GetValue<bool>("Notifications:SMS:Enabled", true))
                {
                    await _smsService.SendSMSAsync(phoneNumber, message);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to send payment overdue notification", ex);
            }
        }

        private async Task SendAbsenceNotificationAsync(string phoneNumber, string employeeName)
        {
            try
            {
                var message = $"NOTICE: {employeeName}, you were absent today. Please provide a reason for your absence.";
                
                if (_configuration.GetValue<bool>("Notifications:WhatsApp:Enabled", true))
                {
                    await _whatsAppService.SendWhatsAppMessageAsync(phoneNumber, message);
                }
                
                if (_configuration.GetValue<bool>("Notifications:SMS:Enabled", true))
                {
                    await _smsService.SendSMSAsync(phoneNumber, message);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to send absence notification", ex);
            }
        }

        private async Task SendAbsenceLimitNotificationAsync(string phoneNumber, string employeeName, int absenceCount)
        {
            try
            {
                var message = $"WARNING: {employeeName}, you have been absent {absenceCount} times this month. Please contact HR immediately.";
                
                if (_configuration.GetValue<bool>("Notifications:WhatsApp:Enabled", true))
                {
                    await _whatsAppService.SendWhatsAppMessageAsync(phoneNumber, message);
                }
                
                if (_configuration.GetValue<bool>("Notifications:SMS:Enabled", true))
                {
                    await _smsService.SendSMSAsync(phoneNumber, message);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to send absence limit notification", ex);
            }
        }
    }
} 
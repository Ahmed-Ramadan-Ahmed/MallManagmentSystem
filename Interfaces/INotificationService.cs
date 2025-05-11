using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface INotificationService
    {
        // Contract Notifications
        Task CheckEmployeeContractExpiryAsync();
        Task CheckStoreContractExpiryAsync();
        
        // Payment Notifications
        Task CheckRenterPaymentOverdueAsync();
        
        // Attendance Notifications
        Task CheckWorkerAbsenceAsync();
        Task CheckWorkerAbsenceLimitAsync();
        
        // General Notification Methods
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsByRecipientAsync(string recipientType, int recipientId);
        Task<List<Notification>> GetUnreadNotificationsAsync(string recipientType, int recipientId);
        Task MarkNotificationAsReadAsync(int notificationId);
        Task MarkAllNotificationsAsReadAsync(string recipientType, int recipientId);
        Task DeleteNotificationAsync(int notificationId);
        Task<int> GetUnreadNotificationCountAsync(string recipientType, int recipientId);
    }
} 
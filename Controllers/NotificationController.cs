using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MallManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("recipient/{recipientType}/{recipientId}")]
        public async Task<ActionResult<List<Notification>>> GetNotifications(string recipientType, int recipientId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsByRecipientAsync(recipientType, recipientId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve notifications", message = ex.Message });
            }
        }

        [HttpGet("unread/{recipientType}/{recipientId}")]
        public async Task<ActionResult<List<Notification>>> GetUnreadNotifications(string recipientType, int recipientId)
        {
            try
            {
                var notifications = await _notificationService.GetUnreadNotificationsAsync(recipientType, recipientId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve unread notifications", message = ex.Message });
            }
        }

        [HttpGet("count/{recipientType}/{recipientId}")]
        public async Task<ActionResult<int>> GetUnreadNotificationCount(string recipientType, int recipientId)
        {
            try
            {
                var count = await _notificationService.GetUnreadNotificationCountAsync(recipientType, recipientId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to get notification count", message = ex.Message });
            }
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                await _notificationService.MarkNotificationAsReadAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to mark notification as read", message = ex.Message });
            }
        }

        [HttpPut("read-all/{recipientType}/{recipientId}")]
        public async Task<IActionResult> MarkAllAsRead(string recipientType, int recipientId)
        {
            try
            {
                await _notificationService.MarkAllNotificationsAsReadAsync(recipientType, recipientId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to mark all notifications as read", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                await _notificationService.DeleteNotificationAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to delete notification", message = ex.Message });
            }
        }

        [HttpPost("check-contracts")]
        public async Task<IActionResult> CheckContracts()
        {
            try
            {
                await _notificationService.CheckEmployeeContractExpiryAsync();
                await _notificationService.CheckStoreContractExpiryAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to check contracts", message = ex.Message });
            }
        }

        [HttpPost("check-payments")]
        public async Task<IActionResult> CheckPayments()
        {
            try
            {
                await _notificationService.CheckRenterPaymentOverdueAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to check payments", message = ex.Message });
            }
        }

        [HttpPost("check-attendance")]
        public async Task<IActionResult> CheckAttendance()
        {
            try
            {
                await _notificationService.CheckWorkerAbsenceAsync();
                await _notificationService.CheckWorkerAbsenceLimitAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to check attendance", message = ex.Message });
            }
        }
    }
} 
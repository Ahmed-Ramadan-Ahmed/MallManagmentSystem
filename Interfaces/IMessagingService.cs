using System.Threading.Tasks;

namespace MallManagmentSystem.Interfaces
{
    public interface IMessagingService
    {
        Task<bool> SendWhatsAppMessageAsync(string phoneNumber, string message);
        Task<bool> SendSMSAsync(string phoneNumber, string message);
        Task<bool> SendBulkWhatsAppMessagesAsync(List<string> phoneNumbers, string message);
        Task<bool> SendBulkSMSAsync(List<string> phoneNumbers, string message);
    }
} 
using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using System.Threading.Tasks;

namespace MallManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IMessagingService _whatsAppService;
        private readonly IMessagingService _smsService;

        public MessagingController(
            [FromKeyedServices("WhatsApp")] IMessagingService whatsAppService,
            [FromKeyedServices("SMS")] IMessagingService smsService)
        {
            _whatsAppService = whatsAppService;
            _smsService = smsService;
        }

        [HttpPost("whatsapp")]
        public async Task<IActionResult> SendWhatsAppMessage([FromBody] MessageRequest request)
        {
            var result = await _whatsAppService.SendWhatsAppMessageAsync(request.PhoneNumber, request.Message);
            return result ? Ok() : BadRequest("Failed to send WhatsApp message");
        }

        [HttpPost("sms")]
        public async Task<IActionResult> SendSMS([FromBody] MessageRequest request)
        {
            var result = await _smsService.SendSMSAsync(request.PhoneNumber, request.Message);
            return result ? Ok() : BadRequest("Failed to send SMS");
        }

        [HttpPost("whatsapp/bulk")]
        public async Task<IActionResult> SendBulkWhatsAppMessages([FromBody] BulkMessageRequest request)
        {
            var result = await _whatsAppService.SendBulkWhatsAppMessagesAsync(request.PhoneNumbers, request.Message);
            return result ? Ok() : BadRequest("Failed to send bulk WhatsApp messages");
        }

        [HttpPost("sms/bulk")]
        public async Task<IActionResult> SendBulkSMS([FromBody] BulkMessageRequest request)
        {
            var result = await _smsService.SendBulkSMSAsync(request.PhoneNumbers, request.Message);
            return result ? Ok() : BadRequest("Failed to send bulk SMS");
        }
    }

    public class MessageRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }

    public class BulkMessageRequest
    {
        public List<string> PhoneNumbers { get; set; }
        public string Message { get; set; }
    }
} 
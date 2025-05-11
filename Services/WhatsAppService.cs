using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MallManagmentSystem.Interfaces;

namespace MallManagmentSystem.Services
{
    public class WhatsAppService : IMessagingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly IConfiguration _configuration;

        public WhatsAppService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["WhatsApp:ApiKey"];
            _apiUrl = _configuration["WhatsApp:ApiUrl"];

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<bool> SendWhatsAppMessageAsync(string phoneNumber, string message)
        {
            try
            {
                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = phoneNumber,
                    type = "text",
                    text = new { body = message }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(_apiUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error sending WhatsApp message: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendSMSAsync(string phoneNumber, string message)
        {
            // This method is not implemented for WhatsApp service
            throw new NotImplementedException("SMS sending is not supported by WhatsApp service");
        }

        public async Task<bool> SendBulkWhatsAppMessagesAsync(List<string> phoneNumbers, string message)
        {
            try
            {
                var tasks = phoneNumbers.Select(phone => SendWhatsAppMessageAsync(phone, message));
                var results = await Task.WhenAll(tasks);
                return results.All(r => r);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error sending bulk WhatsApp messages: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendBulkSMSAsync(List<string> phoneNumbers, string message)
        {
            // This method is not implemented for WhatsApp service
            throw new NotImplementedException("Bulk SMS sending is not supported by WhatsApp service");
        }
    }
} 
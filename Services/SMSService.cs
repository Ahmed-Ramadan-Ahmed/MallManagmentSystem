using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MallManagmentSystem.Interfaces;

namespace MallManagmentSystem.Services
{
    public class SMSService : IMessagingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _senderId;
        private readonly IConfiguration _configuration;

        public SMSService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["SMS:ApiKey"];
            _apiUrl = _configuration["SMS:ApiUrl"];
            _senderId = _configuration["SMS:SenderId"];

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<bool> SendSMSAsync(string phoneNumber, string message)
        {
            try
            {
                var payload = new
                {
                    sender = _senderId,
                    recipient = phoneNumber,
                    message = message
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
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendWhatsAppMessageAsync(string phoneNumber, string message)
        {
            // This method is not implemented for SMS service
            throw new NotImplementedException("WhatsApp messaging is not supported by SMS service");
        }

        public async Task<bool> SendBulkSMSAsync(List<string> phoneNumbers, string message)
        {
            try
            {
                var payload = new
                {
                    sender = _senderId,
                    recipients = phoneNumbers,
                    message = message
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_apiUrl}/bulk", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error sending bulk SMS: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendBulkWhatsAppMessagesAsync(List<string> phoneNumbers, string message)
        {
            // This method is not implemented for SMS service
            throw new NotImplementedException("Bulk WhatsApp messaging is not supported by SMS service");
        }
    }
} 
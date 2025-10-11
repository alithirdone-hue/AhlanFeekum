using AhlanFeekum.FireBase;
using AhlanFeekum.UserNotifications;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;



namespace AhlanFeekum.WhatsApp
{


    public class WhatsAppService : ApplicationService, IWhatsAppAppService
    {
        private readonly ILogger<FcmNotificationService> _logger;
        private readonly IConfiguration _configuration;

        public WhatsAppService(ILogger<FcmNotificationService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> SendMessage(string message)
        {
            var defaultPhoneNumber = _configuration["WhatsApp:DefaultPhoneNumber"] ?? "+963931846622";
            return await SendMessage(message, defaultPhoneNumber);
        }

        public async Task<bool> SendMessage(string message, string phoneNumber)
        {
            // Get configuration from appsettings.json
            var apiKey = _configuration["WhatsApp:ApiKey"];
            var username = _configuration["WhatsApp:Username"];
            var password = _configuration["WhatsApp:Password"];
            var apiUrl = _configuration["WhatsApp:ApiUrl"] ?? "https://wha.cyberv.it.com/api/v1/send-message";
            
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogError("WhatsApp API key is not configured in appsettings.json");
                return false;
            }

            var fullApiUrl = $"{apiUrl}?token={apiKey}";

            // Construct the message payload
            //var payload = new
            //{
            //    messageObject = new
            //    {
            //        to = "00963931846622",
            //        type = "text",
            //        text = new
            //        {
            //            preview_url = false,
            //            body = message
            //        }
            //    }
            //};
            // Create payload - try different approaches based on authentication method
            object payload;
            
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                // Option 1: Include credentials in payload
                payload = new
                {
                    username = username,
                    password = password,
                    messageObject = new
                    {
                        to = phoneNumber,
                        type = "text",
                        text = new
                        {
                            preview_url = false,
                            body = message
                        }
                    }
                };
            }
            else
            {
                // Option 2: Standard payload with token
                payload = new
                {
                    messageObject = new
                    {
                        to = phoneNumber,
                        type = "text",
                        text = new
                        {
                            preview_url = false,
                            body = message
                        }
                    }
                };
            }

            using (var httpClient = new HttpClient())
            {
                // Add basic authentication if username and password are provided
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    _logger.LogInformation("Using Basic Authentication for Ultra Message WhatsApp API");
                }
                else
                {
                    _logger.LogInformation("Using Token Authentication for Ultra Message WhatsApp API");
                }
                
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    _logger.LogInformation("Sending WhatsApp message to {PhoneNumber} via Ultra Message API", phoneNumber);
                    var response = await httpClient.PostAsync(fullApiUrl, content);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    
                    _logger.LogInformation("WhatsApp API Response - Status: {StatusCode}, Response: {Response}", response.StatusCode, responseBody);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to send WhatsApp message. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseBody);
                        return false;
                    }
                    
                    // Parse the response to check if the message was actually sent
                    try
                    {
                        var responseJson = JsonSerializer.Deserialize<JsonElement>(responseBody);
                        
                        // Check if the response indicates success
                        if (responseJson.TryGetProperty("success", out var successElement) && successElement.GetBoolean())
                        {
                            _logger.LogInformation("WhatsApp message sent successfully. Response: {Response}", responseBody);
                            return true;
                        }
                        else if (responseJson.TryGetProperty("success", out var successElement2) && !successElement2.GetBoolean())
                        {
                            // Handle explicit success: false responses
                            if (responseJson.TryGetProperty("message", out var messageElement))
                            {
                                var errorMessage = messageElement.GetString();
                                _logger.LogError("WhatsApp API returned failure: {Message}", errorMessage);
                                
                                // Check for specific error messages
                                if (errorMessage.Contains("META API keys"))
                                {
                                    _logger.LogError("WhatsApp API configuration issue: META API keys not properly configured in profile section");
                                }
                                else if (errorMessage.Contains("token"))
                                {
                                    _logger.LogError("WhatsApp API token issue: Token may be invalid or expired");
                                }
                            }
                            return false;
                        }
                        else if (responseJson.TryGetProperty("error", out var errorElement))
                        {
                            _logger.LogError("WhatsApp API returned error: {Error}", errorElement.GetString());
                            return false;
                        }
                        else if (responseJson.TryGetProperty("status", out var statusElement))
                        {
                            var status = statusElement.GetString();
                            if (status == "sent" || status == "delivered")
                            {
                                _logger.LogInformation("WhatsApp message status: {Status}. Response: {Response}", status, responseBody);
                                return true;
                            }
                            else
                            {
                                _logger.LogWarning("WhatsApp message status: {Status}. Response: {Response}", status, responseBody);
                                return false;
                            }
                        }
                        else
                        {
                            // If we can't determine the status, log the response and return false to be safe
                            _logger.LogWarning("Unable to determine WhatsApp message status. Response: {Response}", responseBody);
                            return false;
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "Failed to parse WhatsApp API response: {Response}", responseBody);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while sending WhatsApp message.");
                    return false;
                }
            }
           
        }

        public async Task<bool> SendTemplateMessage(string sendTo, string templateName, string[] exampleArr, string mediaUri = null)
        {
            var apiKey = _configuration["WhatsApp:ApiKey"];
            return await SendTemplateMessage(sendTo, templateName, exampleArr, apiKey, mediaUri);
        }

        public async Task<bool> SendTemplateMessage(string sendTo, string templateName, string[] exampleArr, string token, string mediaUri = null)
        {
            // Get configuration from appsettings.json
            var apiKey = _configuration["WhatsApp:ApiKey"];
            var apiUrl = _configuration["WhatsApp:TemplateApiUrl"] ?? "https://wha.cyberv.it.com/api/v1/send_templet";
            
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogError("WhatsApp API key is not configured in appsettings.json");
                return false;
            }

            // Create template message payload according to Ultra Message API specification
            var payload = new
            {
                sendTo = sendTo,
                templetName = templateName,
                exampleArr = exampleArr ?? new string[0],
                token = token ?? apiKey,
                mediaUri = mediaUri
            };

            using (var httpClient = new HttpClient())
            {
                // Add Bearer token authorization
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    _logger.LogInformation("Sending WhatsApp template message '{TemplateName}' to {PhoneNumber} via Ultra Message API", templateName, sendTo);
                    var response = await httpClient.PostAsync(apiUrl, content);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    
                    _logger.LogInformation("WhatsApp Template API Response - Status: {StatusCode}, Response: {Response}", response.StatusCode, responseBody);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to send WhatsApp template message. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseBody);
                        return false;
                    }
                    
                    // Parse the response to check if the template message was actually sent
                    try
                    {
                        var responseJson = JsonSerializer.Deserialize<JsonElement>(responseBody);
                        
                        // Check if the response indicates success
                        if (responseJson.TryGetProperty("success", out var successElement) && successElement.GetBoolean())
                        {
                            _logger.LogInformation("WhatsApp template message sent successfully. Response: {Response}", responseBody);
                            return true;
                        }
                        else if (responseJson.TryGetProperty("success", out var successElement2) && !successElement2.GetBoolean())
                        {
                            // Handle explicit success: false responses
                            if (responseJson.TryGetProperty("message", out var messageElement))
                            {
                                var errorMessage = messageElement.GetString();
                                _logger.LogError("WhatsApp Template API returned failure: {Message}", errorMessage);
                                
                                // Check for specific error messages
                                if (errorMessage.Contains("template"))
                                {
                                    _logger.LogError("WhatsApp template issue: Template '{TemplateName}' may not exist or be approved", templateName);
                                }
                                else if (errorMessage.Contains("token"))
                                {
                                    _logger.LogError("WhatsApp API token issue: Token may be invalid or expired");
                                }
                            }
                            return false;
                        }
                        else if (responseJson.TryGetProperty("error", out var errorElement))
                        {
                            _logger.LogError("WhatsApp Template API returned error: {Error}", errorElement.GetString());
                            return false;
                        }
                        else if (responseJson.TryGetProperty("status", out var statusElement))
                        {
                            var status = statusElement.GetString();
                            if (status == "sent" || status == "delivered")
                            {
                                _logger.LogInformation("WhatsApp template message status: {Status}. Response: {Response}", status, responseBody);
                                return true;
                            }
                            else
                            {
                                _logger.LogWarning("WhatsApp template message status: {Status}. Response: {Response}", status, responseBody);
                                return false;
                            }
                        }
                        else
                        {
                            // If we can't determine the status, log the response and return false to be safe
                            _logger.LogWarning("Unable to determine WhatsApp template message status. Response: {Response}", responseBody);
                            return false;
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "Failed to parse WhatsApp Template API response: {Response}", responseBody);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while sending WhatsApp template message.");
                    return false;
                }
            }
        }
    }
}

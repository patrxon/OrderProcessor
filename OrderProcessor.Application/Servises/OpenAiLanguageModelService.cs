using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using OrderProcessor.Application.Interfaces;
using OrderProcessor.Domain.DTOs;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OrderProcessor.Application.Services
{
    public class OpenAiLanguageModelService : ILanguageModelService
    {
        private readonly string _apiKey;
        private readonly string _model;

        public OpenAiLanguageModelService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"]!;
            _model = configuration["OpenAI:Model"]!;
        }

        public async Task<List<OrderInformation>> ExtractOrderInfoAsync(string emailContent)
        {
            var client = new ChatClient(_model, _apiKey);

            var prompt = $$"""
                Extract data from a message.
                Return an array containing such objects:
                {
                    "ProductName": string
                    "Quantity": number
                    "Price": number
                }
                
                Message:
                {{emailContent}}
                """;

            var message = string.Empty;
            List<OrderInformation> products = new List<OrderInformation>();

            try
            {
                var returnMessage = await client.CompleteChatAsync(prompt);

                if (!returnMessage.Value.Content.Any())
                    throw new Exception("No Content");

                message = returnMessage.Value.Content[0].Text;
                var sanitizedMessage = Regex.Match(message, @"\[\s*{[\s\S]*?}\s*\]").Value;
                products = JsonSerializer.Deserialize<List<OrderInformation>>(sanitizedMessage);
            }
            catch (Exception ex)
            { 
            }
    
            return products;
        }
    }
}

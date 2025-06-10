using OrderProcessor.Application.Interfaces;
using OrderProcessor.Domain.DTOs;
using OrderProcessor.Domain.Entities;
using OrderProcessor.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OrderProcessor.Application.Services
{
    public class EmailProcessingService : IEmailProcessingService
    {
        private readonly IEmailEntityRepository _emailEntityRepository;
        private readonly ILanguageModelService _languageModel;

        public EmailProcessingService(IEmailEntityRepository emailEntityRepository, ILanguageModelService languageModel)
        {
            _emailEntityRepository = emailEntityRepository;
            _languageModel = languageModel;
        }

        public async Task<List<OrderInformation>> ProcessEmails()
        {
            var emails = await _emailEntityRepository.GetAllAsync();

            List<OrderInformation> allOrders = new List<OrderInformation>();

            foreach (var email in emails)
            {
                allOrders.AddRange(await ProcessEmailAsync(email));
            }

            return allOrders;
        }

        public async Task<List<OrderInformation>> ProcessEmailAsync(EmailEntity email)
        {
            string emailText = StripHtml(email.BodyHtml ?? string.Empty);

            var orders = await _languageModel.ExtractOrderInfoAsync(emailText);

            return orders;
        }

        private string StripHtml(string html)
        {
            string content = Regex.Replace(html, "<.*?>", "_");
            content = Regex.Replace(content, @"\t|\n|\r", "");
            return content;
        }
    }
}

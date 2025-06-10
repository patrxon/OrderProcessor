using OrderProcessor.Domain.DTOs;
using OrderProcessor.Domain.Entities;

namespace OrderProcessor.Application.Interfaces
{
    public interface IEmailProcessingService
    {
        public Task<List<OrderInformation>> ProcessEmails();
        public Task<List<OrderInformation>> ProcessEmailAsync(EmailEntity emailEntity);
    }
}


using OrderProcessor.Domain.DTOs;

namespace OrderProcessor.Application.Interfaces
{
    public interface ILanguageModelService
    {
        public Task<List<OrderInformation>> ExtractOrderInfoAsync(string emailContent);
    }
}

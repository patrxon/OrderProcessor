
namespace OrderProcessor.Application.Interfaces
{
    public interface IImapMailService
    {
        public Task LoadEmailsAsync();
        public Task StoreEmailsAsync();
    }
}

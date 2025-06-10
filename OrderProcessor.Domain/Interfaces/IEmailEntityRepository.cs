using OrderProcessor.Domain.Entities;


namespace OrderProcessor.Domain.Interfaces
{
    public interface IEmailEntityRepository
    {
        Task<IEnumerable<EmailEntity>> GetAllAsync();
        Task AddAsync(EmailEntity entity);
        Task SaveChangesAsync();
    }
}


using Microsoft.EntityFrameworkCore;
using OrderProcessor.Domain.Entities;
using OrderProcessor.Domain.Interfaces;

namespace OrderProcessor.Infrastructure.Persistence
{
    public class EmailEntityRepository : IEmailEntityRepository
    {
        private readonly AppDbContext _dbContext;

        public EmailEntityRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(EmailEntity entity)
        {
            _dbContext.Emails.Add(entity);
        }

        public async Task<IEnumerable<EmailEntity>> GetAllAsync()
        {
            return await _dbContext.Emails.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

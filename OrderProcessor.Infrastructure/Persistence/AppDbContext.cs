using Microsoft.EntityFrameworkCore;
using OrderProcessor.Domain.Entities;

namespace OrderProcessor.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EmailEntity> Emails { get; set; }
    }
}

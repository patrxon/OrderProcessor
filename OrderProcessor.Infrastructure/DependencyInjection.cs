using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessor.Domain.Interfaces;
using OrderProcessor.Infrastructure.Persistence;

namespace OrderProcessor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var conString = config.GetConnectionString("MySqlConnection");
            services.AddScoped<IEmailEntityRepository, EmailEntityRepository>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(conString, new MySqlServerVersion(new Version(8, 0, 36))));

            return services;
        }
    }
}

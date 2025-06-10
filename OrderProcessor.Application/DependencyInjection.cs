using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessor.Application.Interfaces;
using OrderProcessor.Application.Services;

namespace OrderProcessor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ILanguageModelService, OpenAiLanguageModelService>();
            services.AddScoped<IImapMailService, ImapMailService>();
            services.AddScoped<IEmailProcessingService, EmailProcessingService>();

            return services;
        }
    }
}

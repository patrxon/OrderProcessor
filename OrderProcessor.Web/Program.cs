using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using OrderProcessor.Infrastructure;
using OrderProcessor.Infrastructure.Persistence;
using OrderProcessor.Web.Components;

namespace OrderProcessor.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMudServices();


            builder.WebHost.UseUrls("http://+:80");


            var app = builder.Build();

            try
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration failed: {ex.Message}");
                throw;
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Sparplan.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using Sparplan.Application.Services; 
using Sparplan.Infrastructure.Services; 
using Sparplan.Infrastructure.Background;

namespace Sparplan.Api
{
    /// <summary>
    /// Konfigurationsklasse für die ASP.NET Core Anwendung.
    /// Registriert Services (DI) und richtet die Middleware-Pipeline ein.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Registriert Services in den Dependency Injection Container,
        /// u. a. Controller, Swagger und die PostgreSQL-Datenbankanbindung.
        /// </summary>
        /// <param name="services">Die ServiceCollection, in die die Registrierungen erfolgen.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Controller mit JSON-Optionen (Enums als Strings serialisieren)
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });

            // Swagger/OpenAPI aktivieren
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sparplan API",
                    Version = "v1"
                });
            });

            // PostgreSQL-Datenbank anbinden (ConnectionString aus Environment-Variablen)
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // HttpClient for the scraper
            services.AddHttpClient();
            // DI: scraper + history
            services.AddScoped<IMetalPriceService, MetalPriceScraper>();
            services.AddScoped<IMetalPriceHistoryService, MetalPriceHistoryService>();
            // background worker
            services.AddHostedService<PriceFetchWorker>();

        }

        /// <summary>
        /// Konfiguriert die HTTP-Request-Pipeline (Middleware).
        /// Enthält Swagger, HTTPS-Redirect, Authorization und Controller-Mapping.
        /// </summary>
        /// <param name="app">Die WebApplication-Instanz.</param>
        /// <param name="env">Umgebungsinformationen (Development/Production).</param>
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            // Swagger aktivieren
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sparplan API v1");
                c.RoutePrefix = "swagger"; // Swagger unter http://localhost:5001/swagger erreichbar
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Controller-Endpunkte registrieren
            app.MapControllers();
        }
    }
}

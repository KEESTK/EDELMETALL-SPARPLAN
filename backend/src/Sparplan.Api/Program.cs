using Sparplan.Api;
using Sparplan.Infrastructure.Services;

/// <summary>
/// Einstiegspunkt der Anwendung.
/// Initialisiert die WebApplication, verwendet die Startup-Klasse
/// zur Konfiguration von Services und Middleware
/// und startet den Kestrel-Webserver.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// === CORS policy ===
// Allow Angular frontend during development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",   // Local Angular dev
                "http://localhost:5000",   // Optional: backend test via Nginx reverse proxy
                "http://localhost:5001"    // Optional: backend standalone
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // important if using cookies or auth headers
    });
});

// === Register application services (via Startup) ===
var startup = new Startup();
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// === Apply CORS before routing / endpoints ===
app.UseCors("AllowFrontend");

// === Developer tools ===
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// === Configure pipeline (from Startup) ===
startup.Configure(app, builder.Environment);

// === Run application ===
app.Run();

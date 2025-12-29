using Analytics.API.Controllers;
using Analytics.Infrastructure;
using Faculty.Infrastructure.Db;
using Faculty.Infrastructure.DependencyInjection;
using Identity.API.Controllers;
using Identity.Infrastructure.Db;
using Identity.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Controllers;
using Notifications.Infrastructure;
using Support.API.Controllers;
using Support.Infrastructure;
using Support.Infrastructure.Db;
using University.API.Controllers;
using University.Infrastructure;
using University.Infrastructure.Db;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using FacultyController = Faculty.API.Controllers.FacultyController;

// ---------------------------------------------------------
// PRE-BUILDER CONFIGURATION
// Forcefully clear HTTPS environment variables to stop Kestrel 
// from looking for certificates based on defaults.
// ---------------------------------------------------------
var portStr = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var port = int.Parse(portStr);

// Force the URL environment variable to HTTP only
Environment.SetEnvironmentVariable("ASPNETCORE_URLS", $"http://0.0.0.0:{port}");
// Clear the HTTPS port variable to prevent default binding
Environment.SetEnvironmentVariable("ASPNETCORE_HTTPS_PORT", ""); 

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// KESTREL MANUAL CONFIGURATION
// This ignores appsettings.json "Kestrel" section and 
// forces a single HTTP listener.
// ---------------------------------------------------------
builder.WebHost.ConfigureKestrel(options =>
{
    // Listen strictly on the Render port (IPv4/IPv6)
    // This creates an endpoint WITHOUT TLS/SSL.
    options.ListenAnyIP(port);
});
// ---------------------------------------------------------

// Add services from modules
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddUniversityModule(builder.Configuration);
builder.Services.AddFacultyModule(builder.Configuration);
builder.Services.AddSupportModule(builder.Configuration);
builder.Services.AddNotificationsModule();
builder.Services.AddAnalyticsModule();

var mvcBuilder = builder.Services.AddControllers();

var moduleControllers = new[]
{
    typeof(IdentityController).Assembly,
    typeof(UniversityController).Assembly,
    typeof(FacultyController).Assembly,
    typeof(SupportController).Assembly,
    typeof(NotificationsController).Assembly,
    typeof(AnalyticsController).Assembly
};

foreach (var asm in moduleControllers)
{
    mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(asm));
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
    foreach (var xmlPath in xmlFiles)
    {
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

var app = builder.Build();

// Forwarded Headers (Required for Render)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

var applyMigrations = false;

if (applyMigrations)
{
    using (var scope = app.Services.CreateScope())
    {
        // Migrations logic omitted
    }
}

// DO NOT use HttpsRedirection
// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

Console.WriteLine($"Starting server strictly on port {port} (HTTP)...");

app.Run();
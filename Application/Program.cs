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

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// FORCE HTTP ONLY (CRITICAL FIX FOR RENDER)
// This overrides any default settings trying to bind HTTPS
// ---------------------------------------------------------
var portVar = Environment.GetEnvironmentVariable("PORT");
var port = string.IsNullOrEmpty(portVar) ? 8080 : int.Parse(portVar);

builder.WebHost.ConfigureKestrel(options =>
{
    // Listen on Any IP (0.0.0.0) on the port assigned by Render
    options.Listen(IPAddress.Any, port);
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

// Disable automatic migrations on startup to prevent crashes
var applyMigrations = false;

if (applyMigrations)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        // Migration logic omitted to prevent startup crashes
    }
}

// DISABLED HTTPS REDIRECTION
// Render handles SSL termination externally; internal traffic must be HTTP.
// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger in production for testing
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

Console.WriteLine($"Starting web server on port {port} (HTTP)...");

app.Run();
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

// Render environment configuration
// Render gives us a PORT env var. We must listen on 0.0.0.0:{PORT}
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

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

// Configure the HTTP request pipeline.

// 1. Forwarded Headers are CRITICAL for Render
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// 2. Disable HTTPS Redirection in Docker/Render
// app.UseHttpsRedirection(); 

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
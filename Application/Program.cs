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
using Microsoft.AspNetCore.HttpOverrides; // Added for Render
using FacultyController = Faculty.API.Controllers.FacultyController;

var builder = WebApplication.CreateBuilder(args);

// Add services from modules
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddUniversityModule(builder.Configuration);
builder.Services.AddFacultyModule(builder.Configuration);
builder.Services.AddSupportModule(builder.Configuration);
builder.Services.AddNotificationsModule();
builder.Services.AddAnalyticsModule();

// Add controllers and module API assemblies
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

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Load all XML documentation files (e.g. Application.xml, Identity.API.xml)
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");

    foreach (var xmlPath in xmlFiles)
    {
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

var app = builder.Build();

// ---------------------------------------------------------
// FIX 1: Forwarded Headers for Render
// This allows the app to know it is running behind a proxy (Load Balancer)
// ---------------------------------------------------------
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// ---------------------------------------------------------
// FIX 2: Disable automatic migrations on startup
// Set this to false to prevent crashes when the database is "sleeping"
// or when HttpContext is unavailable. You should run migrations manually.
// ---------------------------------------------------------
var applyMigrations = false;

if (applyMigrations)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        // Identity module
        try
        {
            var identityDb = services.GetRequiredService<AuthDbContext>();
            identityDb.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error migrating IdentityDbContext: {ex.Message}");
        }

        // University module
        try
        {
            var universityDb = services.GetRequiredService<UniversityDbContext>();
            universityDb.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error migrating UniversityDbContext: {ex.Message}");
        }

        // Faculty module
        try
        {
            var facultyDb = services.GetRequiredService<FacultyDbContext>();
            facultyDb.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error migrating FacultyDbContext: {ex.Message}");
        }

        // Support module
        try
        {
            var supportDb = services.GetRequiredService<SupportDbContext>();
            supportDb.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error migrating SupportDbContext: {ex.Message}");
        }
    }
}

// ---------------------------------------------------------
// FIX 3: Disable HTTPS Redirection
// Render handles SSL/HTTPS externally. The internal container must run on HTTP.
// ---------------------------------------------------------
// app.UseHttpsRedirection(); 

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI();

// Map controllers
app.MapControllers();

Console.WriteLine("Application started successfully on Render.");

app.Run();
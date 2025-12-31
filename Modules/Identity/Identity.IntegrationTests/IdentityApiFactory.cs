using Identity.Infrastructure.Db;
using Identity.API.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.IntegrationTests
{
    public class IdentityApiFactory : WebApplicationFactory<AuthController>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"JwtSettings:Issuer", "IntegrationTest"},
                    {"JwtSettings:Audience", "IntegrationTest"},
                    {"JwtSettings:SigningKey", Convert.ToBase64String(new byte[32])}
                });
            });

            builder.ConfigureServices(services =>
            {
                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();
            });
        }
    }
}

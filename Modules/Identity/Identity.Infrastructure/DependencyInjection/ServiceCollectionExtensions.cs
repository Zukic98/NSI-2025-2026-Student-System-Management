using Identity.Application.Interfaces;
using Identity.Application.Services;
using Identity.Core.Configuration;
using Identity.Core.Interfaces.Repositories;
using Identity.Core.Interfaces.Services;
using Identity.Infrastructure.Db;
using Identity.Infrastructure.Entities;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            var jwtSettings = new JwtSettings();
            configuration.Bind("JwtSettings", jwtSettings);

            if (string.IsNullOrWhiteSpace(jwtSettings.SigningKey))
                throw new InvalidOperationException("JwtSettings:SigningKey is missing.");
            if (string.IsNullOrWhiteSpace(jwtSettings.Issuer))
                throw new InvalidOperationException("JwtSettings:Issuer is missing.");
            if (string.IsNullOrWhiteSpace(jwtSettings.Audience))
                throw new InvalidOperationException("JwtSettings:Audience is missing.");

            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Database")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            services.AddScoped<IdentityDbContextSeed>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var keyBytes = Convert.FromBase64String(jwtSettings.SigningKey);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}

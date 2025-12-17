using Analytics.Application.Services;
using Analytics.Core.Interfaces;
using Analytics.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Analytics.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAnalyticsModule(this IServiceCollection services)
        {
			// Registracija Repozitorija - Infrastructure sloj
			services.AddScoped<IStudentAnalyticsRepository, StudentAnalyticsRepository>();

			// Registracija Servisa - Application sloj
			services.AddScoped<IStudentAnalyticsService, StudentAnalyticsService>();

			return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Analytics.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAnalyticsModule(this IServiceCollection services)
        {
            return services;
        }
    }
}

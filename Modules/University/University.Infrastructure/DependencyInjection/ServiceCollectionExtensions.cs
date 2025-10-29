using Microsoft.Extensions.DependencyInjection;

namespace University.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUniversityModule(this IServiceCollection services)
        {
            return services;
        }
    }
}

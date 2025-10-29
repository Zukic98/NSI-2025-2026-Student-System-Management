using Microsoft.Extensions.DependencyInjection;

namespace Support.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSupportModule(this IServiceCollection services)
        {
            return services;
        }
    }
}

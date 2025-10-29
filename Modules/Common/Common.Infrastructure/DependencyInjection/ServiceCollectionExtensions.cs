using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonModule(this IServiceCollection services)
        {
            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services)
        {
            return services;
        }
    }
}

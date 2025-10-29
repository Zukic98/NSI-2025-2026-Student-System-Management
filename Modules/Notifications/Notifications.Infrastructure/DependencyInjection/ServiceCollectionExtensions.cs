using Microsoft.Extensions.DependencyInjection;

namespace Notifications.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotificationsModule(this IServiceCollection services)
        {
            return services;
        }
    }
}

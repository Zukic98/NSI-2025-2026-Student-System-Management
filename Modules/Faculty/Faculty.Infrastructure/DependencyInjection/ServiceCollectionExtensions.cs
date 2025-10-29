using Microsoft.Extensions.DependencyInjection;

namespace Faculty.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFacultyModule(this IServiceCollection services)
        {
            return services;
        }
    }
}

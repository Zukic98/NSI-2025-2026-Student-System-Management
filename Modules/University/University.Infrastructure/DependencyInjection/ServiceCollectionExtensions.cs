using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace University.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUniversityModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UniversityDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("UniversityConnection")));
       
            return services;
        }
    }
}

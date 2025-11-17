using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Faculty.Infrastructure.Db;

namespace Faculty.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFacultyModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<FacultyDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("FacultyConnection")));

            services.AddMediatR(typeof(CourseCreatedEventHandler).Assembly);

            return services;
        }
    }
}

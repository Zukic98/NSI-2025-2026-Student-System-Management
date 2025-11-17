using Faculty.Infrastructure.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using University.Application.Events;

namespace Faculty.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFacultyModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<FacultyDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("FacultyConnection")));

            services.AddMediatR(typeof(Faculty.Application.EventHandlers.CourseCreatedEventHandler).Assembly);

            return services;
        }
    }
}

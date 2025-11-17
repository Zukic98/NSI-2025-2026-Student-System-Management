using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using University.Core.Interfaces;
using University.Infrastructure.Db;
using University.Infrastructure.Repositories;
using University.Application.Services;
using MediatR;
using MediatR.Registration;
using University.Application.Events;

namespace University.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUniversityModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<UniversityDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("UniversityConnection")));

        services.AddMediatR(typeof(CourseCreatedEvent).Assembly);


        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<CourseService>();

        return services;
    }
}

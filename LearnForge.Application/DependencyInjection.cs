using LearnForge.Application.Courses.CreateCourse;
using LearnForge.Application.Courses.GetCourseById;

using Microsoft.Extensions.DependencyInjection;

namespace LearnForge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateCourseHandler>();
        services.AddScoped<GetCourseByIdHandler>();

        return services;
    }
}
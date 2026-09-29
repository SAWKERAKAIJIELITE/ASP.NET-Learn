using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

using LearnForge.Domain.Enums;
using LearnForge.Infrastructure.Persistence;
using LearnForge.Infrastructure.Persistence.Interceptors;

namespace LearnForge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        dataSourceBuilder.MapEnum<MaterialStatus>();
        dataSourceBuilder.MapEnum<ActivityType>();
        dataSourceBuilder.MapEnum<LessonResourceType>();

        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<LearnForgeDbContext>(options =>
            options.UseNpgsql(dataSource).AddInterceptors(new SoftDeleteInterceptor())
        );

        return services;
    }
}
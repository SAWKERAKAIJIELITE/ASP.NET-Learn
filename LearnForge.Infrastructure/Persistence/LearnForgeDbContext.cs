using LearnForge.Domain.Entities;
using LearnForge.Domain.Enums;
using LearnForge.Infrastructure.Persistence.ReferenceData;

using Microsoft.EntityFrameworkCore;

namespace LearnForge.Infrastructure.Persistence;

public sealed class LearnForgeDbContext(DbContextOptions<LearnForgeDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();

    public DbSet<CourseModule> CourseModules => Set<CourseModule>();

    public DbSet<Lesson> Lessons => Set<Lesson>();

    public DbSet<Activity> Activities => Set<Activity>();

    public DbSet<LessonResource> LessonResources => Set<LessonResource>();

    public DbSet<MaterialStatusLookup> MaterialStatuses => Set<MaterialStatusLookup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<MaterialStatus>();
        modelBuilder.HasPostgresEnum<ActivityType>();
        modelBuilder.HasPostgresEnum<LessonResourceType>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearnForgeDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    // {
    //     foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    //     {
    //         switch (entry.State)
    //         {
    //             // case EntityState.Added:
    //             //     entry.Entity.CreatedAt = DateTime.UtcNow;
    //             //     // entry.Entity.UpdatedAt = DateTime.UtcNow;
    //             //     break;
    //             case EntityState.Modified:
    //                 entry.Entity.UpdatedAt = DateTime.UtcNow;
    //                 break;
    //             case EntityState.Added:
    //             case EntityState.Detached:
    //             case EntityState.Unchanged:
    //             case EntityState.Deleted:
    //                 break;
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //     }

    //     return await base.SaveChangesAsync(cancellationToken);
    // }
}
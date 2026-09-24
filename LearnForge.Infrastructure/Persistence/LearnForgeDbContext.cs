using LearnForge.Domain.Entities;
using LearnForge.Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace LearnForge.Infrastructure.Persistence;

public sealed class LearnForgeDbContext(DbContextOptions<LearnForgeDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();

    public DbSet<CourseModule> CourseModules => Set<CourseModule>();

    public DbSet<Lesson> Lessons => Set<Lesson>();

    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearnForgeDbContext).Assembly);
    }

    // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    // {
    //     foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    //     {
    //         switch (entry.State)
    //         {
    //             case EntityState.Added:
    //                 entry.Entity.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
    //                 // entry.Entity.UpdatedAt = DateTime.UtcNow;
    //                 break;
    //             case EntityState.Modified:
    //                 entry.Entity.LastUpdated = DateOnly.FromDateTime(DateTime.Now);
    //                 break;
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
using LearnForge.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class CourseModuleConfiguration
    : IEntityTypeConfiguration<CourseModule>
{
    public void Configure(EntityTypeBuilder<CourseModule> builder)
    {
        builder.ToTable("course_modules");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

        builder.Property(x => x.Order).IsRequired();

        builder.Property(x => x.CourseId).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.Lessons)
            .WithOne()
            .HasForeignKey(x => x.CourseModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CourseId, x.Order }).IsUnique();
    }
}
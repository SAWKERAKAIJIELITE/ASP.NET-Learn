using LearnForge.Domain.Entities;
using LearnForge.Infrastructure.Persistence.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModule>
{
    public void Configure(EntityTypeBuilder<CourseModule> builder)
    {
        builder.ToTable("course_modules");

        builder.ConfigureOrderedEntity();

        builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);

        builder.Property(x => x.CourseId).IsRequired();

        builder.HasMany(x => x.Lessons)
            .WithOne()
            .HasForeignKey(x => x.CourseModuleId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(x => new { x.CourseId, x.Order }).IsUnique();
    }
}
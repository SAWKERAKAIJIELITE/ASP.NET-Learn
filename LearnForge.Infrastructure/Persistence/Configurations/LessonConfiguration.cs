using LearnForge.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class LessonConfiguration
    : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("lessons");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

        builder.Property(x => x.Order).IsRequired();

        builder.Property(x => x.CourseModuleId).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.Activities)
            .WithOne()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CourseModuleId, x.Order }).IsUnique();
    }
}
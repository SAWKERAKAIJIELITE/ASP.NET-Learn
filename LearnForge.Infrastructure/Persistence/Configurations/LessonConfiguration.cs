using LearnForge.Domain.Entities;
using LearnForge.Infrastructure.Persistence.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("lessons");

        builder.ConfigureOrderedEntity();

        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(2000);

        builder.Property(x => x.ContentMarkdown).IsRequired(false).HasColumnType("text");

        builder.Property(x => x.CourseModuleId).IsRequired();

        builder.HasMany(x => x.Activities)
            .WithOne()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(x => x.Resources)
            .WithOne()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(x => new { x.CourseModuleId, x.Order }).IsUnique();
    }
}
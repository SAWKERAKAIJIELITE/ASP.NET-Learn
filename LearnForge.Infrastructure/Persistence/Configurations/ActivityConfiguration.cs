using LearnForge.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class ActivityConfiguration
    : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("activities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

        builder.Property(x => x.Type).IsRequired();

        builder.Property(x => x.Order).IsRequired();

        builder.Property(x => x.LessonId).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => new { x.LessonId, x.Order }).IsUnique();
    }
}
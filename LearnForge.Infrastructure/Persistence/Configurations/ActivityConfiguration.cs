using LearnForge.Domain.Entities;
using LearnForge.Infrastructure.Persistence.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("activities");

        builder.ConfigureOrderedEntity();

        builder.Property(x => x.Type).IsRequired();

        builder.Property(x => x.Prompt).IsRequired().HasColumnType("text");

        builder.Property(x => x.ContentJson).IsRequired().HasColumnType("text").HasDefaultValue("{}");

        builder.Property(x => x.LessonId).IsRequired();

        builder.HasIndex(x => new { x.Type, x.Prompt, x.ContentJson }).IsUnique();

        builder.ToTable(t =>
            t.HasCheckConstraint($"CK_{nameof(Activity)}_Prompt_NotEmpty", "LENGTH(TRIM(\"Prompt\")) > 0")
        );
    }
}
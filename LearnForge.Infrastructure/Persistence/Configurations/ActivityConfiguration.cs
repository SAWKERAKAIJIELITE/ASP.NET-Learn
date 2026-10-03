using LearnForge.Domain.Entities;
using LearnForge.Infrastructure.Persistence.Extensions;
using LearnForge.Infrastructure.Persistence.ReferenceData;

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
        builder.HasOne<ActivityTypeLookup>()
            .WithMany()
            .HasForeignKey(x => x.Type)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Prompt).IsRequired().HasColumnType("text");

        builder.Property(x => x.ContentJson).IsRequired().HasColumnType("jsonb").HasDefaultValue("{}");

        builder.Property(x => x.LessonId).IsRequired();

        // builder.HasIndex(x => new { x.Type, x.Prompt, x.ContentJson }).IsUnique();
        builder.HasIndex(x => new { x.LessonId, x.Order }).IsUnique();

        builder.ToTable(t =>
            t.HasCheckConstraint($"CK_{nameof(Activity)}_Prompt_NotEmpty", "LENGTH(TRIM(\"Prompt\")) > 0")
        );
    }
}
using LearnForge.Domain.Entities;
using LearnForge.Infrastructure.Persistence.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class LessonResourceConfiguration : IEntityTypeConfiguration<LessonResource>
{
    public void Configure(EntityTypeBuilder<LessonResource> builder)
    {
        builder.ToTable("lesson_resources");

        builder.ConfigureOrderedEntity();

        builder.Property(x => x.Type).HasColumnType("lesson_resource_type").IsRequired();

        builder.Property(x => x.Url).IsRequired().HasMaxLength(2048);

        builder.Property(x => x.LessonId).IsRequired();

        // builder.HasIndex(x => new { x.Title, x.Url, x.Type }).IsUnique();
        builder.HasIndex(x => new { x.LessonId, x.Order }).IsUnique();

        builder.ToTable(t =>
            t.HasCheckConstraint($"CK_{nameof(LessonResource)}_Url_NotEmpty", "LENGTH(TRIM(\"Url\")) > 0")
        );
    }
}
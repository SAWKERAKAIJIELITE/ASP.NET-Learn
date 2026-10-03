using LearnForge.Domain.Enums;
using LearnForge.Infrastructure.Persistence.ReferenceData;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class LessonResourceTypeLookupConfiguration : IEntityTypeConfiguration<LessonResourceTypeLookup>
{
    public void Configure(EntityTypeBuilder<LessonResourceTypeLookup> builder)
    {
        builder.ToTable("lesson_resource_types");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(20);
        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasData(
            new LessonResourceTypeLookup { Id = LessonResourceType.Article, Code = nameof(LessonResourceType.Article) },
            new LessonResourceTypeLookup { Id = LessonResourceType.Video, Code = nameof(LessonResourceType.Video) },
            new LessonResourceTypeLookup { Id = LessonResourceType.Docs, Code = nameof(LessonResourceType.Docs) },
            new LessonResourceTypeLookup { Id = LessonResourceType.Repos, Code = nameof(LessonResourceType.Repos) },
            new LessonResourceTypeLookup { Id = LessonResourceType.Website, Code = nameof(LessonResourceType.Website) }
        );
    }
}
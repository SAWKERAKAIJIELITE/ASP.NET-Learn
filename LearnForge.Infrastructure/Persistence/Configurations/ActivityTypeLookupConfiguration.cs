using LearnForge.Domain.Enums;
using LearnForge.Infrastructure.Persistence.ReferenceData;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class ActivityTypeLookupConfiguration : IEntityTypeConfiguration<ActivityTypeLookup>
{
    public void Configure(EntityTypeBuilder<ActivityTypeLookup> builder)
    {
        builder.ToTable("activity_types");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(20);
        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasData(
            new ActivityTypeLookup { Id = ActivityType.Article, Code = nameof(ActivityType.Article) },
            new ActivityTypeLookup { Id = ActivityType.CodeChallenge, Code = nameof(ActivityType.CodeChallenge) },
            new ActivityTypeLookup { Id = ActivityType.DebugChallenge, Code = nameof(ActivityType.DebugChallenge) },
            new ActivityTypeLookup { Id = ActivityType.MultipleChoice, Code = nameof(ActivityType.MultipleChoice) },
            new ActivityTypeLookup { Id = ActivityType.FillInBlank, Code = nameof(ActivityType.FillInBlank) },
            new ActivityTypeLookup { Id = ActivityType.CodeOrdering, Code = nameof(ActivityType.CodeOrdering) },
            new ActivityTypeLookup { Id = ActivityType.OutputPrediction, Code = nameof(ActivityType.OutputPrediction) }
        );
    }
}
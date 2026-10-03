using LearnForge.Domain.Enums;

namespace LearnForge.Infrastructure.Persistence.ReferenceData;

public sealed class ActivityTypeLookup
{
    public ActivityType Id { get; set; }
    public string Code { get; set; } = null!;
}
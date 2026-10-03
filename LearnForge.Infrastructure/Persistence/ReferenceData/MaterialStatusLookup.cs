using LearnForge.Domain.Enums;

namespace LearnForge.Infrastructure.Persistence.ReferenceData;

public sealed class MaterialStatusLookup
{
    public MaterialStatus Id { get; set; }
    public string Code { get; set; } = null!;
}
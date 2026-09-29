namespace LearnForge.Domain.Common;

public class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; internal set; }

    public DateTime? DeletedAt { get; internal set; }
}

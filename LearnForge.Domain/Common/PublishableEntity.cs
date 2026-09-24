using LearnForge.Domain.Enums;

namespace LearnForge.Domain.Common;

public abstract class PublishableEntity : BaseEntity
{
    public MaterialStatus Status { get; private set; } = MaterialStatus.Draft;

    public void Publish()
    {
        EnsureCanPublish();
        if (Status == MaterialStatus.Published) return;
        Status = MaterialStatus.Published;
    }

    public void Unpublish()
    {
        if (Status == MaterialStatus.Draft) return;
        Status = MaterialStatus.Draft;
    }

    protected virtual void EnsureCanPublish() { }
}
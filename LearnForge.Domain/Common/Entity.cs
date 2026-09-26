using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;


namespace LearnForge.Domain.Common;

public abstract class Entity : BaseEntity
{
    public string Title { get; private set; }

    public MaterialStatus Status { get; private set; } = MaterialStatus.Draft;

    protected Entity() { }
    protected Entity(string title) => Title = ValidateTitle(title);

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

    public void Rename(string title) => Title = ValidateTitle(title);

    private string ValidateTitle(string title)
    {
        return string.IsNullOrWhiteSpace(title)
            ? throw new DomainException($"{EntityLabel} title is required.")
            : title.Trim();
    }

    protected virtual string EntityLabel => GetType().Name;
}
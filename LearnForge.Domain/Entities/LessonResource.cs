using LearnForge.Domain.Common;
using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;

namespace LearnForge.Domain.Entities;

public sealed class LessonResource : PublishableEntity
{
    private LessonResource() { }

    internal LessonResource(Guid lessonId, string title, string url, int order, LessonResourceType type)
    {
        if (lessonId == Guid.Empty)
            throw new DomainException("Lesson is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Resource title is required.");

        if (!Enum.IsDefined(type))
            throw new DomainException("Invalid Resource type.");

        if (
            !Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        )
            throw new DomainException("Resource URL must be a valid http or https address.");

        if (order < 1)
            throw new DomainException("Resource order must be greater than zero.");

        LessonId = lessonId;
        Title = title.Trim();
        Url = uri.AbsoluteUri;
        Order = order;
        Type = type;
    }

    public Guid LessonId { get; private set; }

    public string Title { get; private set; } = null!;

    public string Url { get; private set; } = null!;

    public LessonResourceType Type { get; private set; }

    public int Order { get; private set; }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Resource title is required.");

        Title = title.Trim();
    }

    public void UpdateUrl(string url)
    {
        if (
            !Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        )
            throw new DomainException("Resource URL must be a valid http or https address.");

        Url = uri.AbsoluteUri;
    }

    public void UpdateType(LessonResourceType type)
    {
        if (!Enum.IsDefined(type))
            throw new DomainException("Invalid Resource type.");

        Type = type;
    }

    public void ChangeOrder(int order)
    {
        if (order < 1)
            throw new DomainException("Resource order must be greater than zero.");

        Order = order;
    }
}
using LearnForge.Domain.Common;
using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;

namespace LearnForge.Domain.Entities;

public sealed class LessonResource : OrderedEntity
{
    private LessonResource() { }

    internal LessonResource(
        Guid lessonId,
        string title,
        string url,
        int order,
        LessonResourceType type
    ) : base(title, order)
    {
        if (lessonId == Guid.Empty)
            throw new DomainException("Lesson is required.");

        LessonId = lessonId;
        Url = ConvertUrlStringToAbsoluteUri(url);
        Type = ValidateResourceType(type);
    }

    public Guid LessonId { get; private set; }

    public string Url { get; private set; }

    public LessonResourceType Type { get; private set; }

    private static bool CheckUrlString(string url, out Uri uri)
    {
        return
            !Uri.TryCreate(url.Trim(), UriKind.Absolute, out uri!) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps);
    }

    private static string ConvertUrlStringToAbsoluteUri(string url)
    {
        return CheckUrlString(url, out Uri uri)
            ? throw new DomainException("Resource URL must be a valid http or https address.")
            : uri.AbsoluteUri;
    }

    private static LessonResourceType ValidateResourceType(LessonResourceType type)
    {
        return !Enum.IsDefined(type) ? throw new DomainException("Invalid Resource type.") : type;
    }

    public void UpdateUrl(string url) => Url = ConvertUrlStringToAbsoluteUri(url);

    public void UpdateType(LessonResourceType type) => Type = ValidateResourceType(type);
}
using LearnForge.Domain.Common;
using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;

namespace LearnForge.Domain.Entities;

public sealed class Activity : PublishableEntity
{
    private Activity()
    {
    }

    internal Activity(
        Guid lessonId,
        ActivityType type,
        string title,
        int order,
        string prompt,
        string contentJson
    )
    {
        if (lessonId == Guid.Empty)
            throw new DomainException("Lesson is required.");

        if (!Enum.IsDefined(type))
            throw new DomainException("Invalid activity type.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Activity title is required.");
        // you can do this instead
        // ArgumentException.ThrowIfNullOrWhiteSpace(title);

        if (string.IsNullOrWhiteSpace(prompt))
            throw new DomainException("Activity prompt is required.");

        if (order < 1)
            throw new DomainException("Activity order must be greater than zero.");

        LessonId = lessonId;
        Type = type;
        Title = title.Trim();
        Prompt = prompt.Trim();
        Order = order;
        ContentJson = contentJson;
    }

    public Guid LessonId { get; private set; }

    public ActivityType Type { get; private set; }

    public string Title { get; private set; } = null!;

    public int Order { get; private set; }

    public string Prompt { get; private set; } = null!;

    public string ContentJson { get; private set; } = "{}";

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Activity title is required.");

        Title = title.Trim();
    }

    public void ChangePrompt(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new DomainException("Activity prompt is required.");

        Prompt = prompt.Trim();
    }

    public void UpdateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Activity content is required.");

        ContentJson = content.Trim();
    }

    public void ChangeOrder(int order)
    {
        if (order < 1)
            throw new DomainException("Activity order must be greater than zero.");

        Order = order;
    }
}
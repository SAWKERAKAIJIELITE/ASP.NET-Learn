using LearnForge.Domain.Common;
using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;

namespace LearnForge.Domain.Entities;

public sealed class Activity : OrderedEntity
{
    private Activity() { }

    internal Activity(
        Guid lessonId,
        ActivityType type,
        string title,
        int order,
        string prompt,
        string contentJson
    ) : base(title, order)
    {
        if (lessonId == Guid.Empty)
            throw new DomainException("Lesson is required.");

        if (!Enum.IsDefined(type))
            throw new DomainException("Invalid activity type.");

        LessonId = lessonId;
        Type = type;
        Prompt = ValidatePrompt(prompt);
        ContentJson = contentJson;
    }

    public Guid LessonId { get; private set; }

    public ActivityType Type { get; private set; }

    public string Prompt { get; private set; } = null!;

    public string ContentJson { get; private set; } = "{}";

    public void ChangePrompt(string prompt) => Prompt = ValidatePrompt(prompt);

    private static string ValidatePrompt(string prompt)
    {
        return string.IsNullOrWhiteSpace(prompt)
            ? throw new DomainException("Activity prompt is required.")
            : prompt.Trim();
    }

    public void UpdateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Activity content is required.");

        ContentJson = content.Trim();
    }
}
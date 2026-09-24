using LearnForge.Domain.Common;
using LearnForge.Domain.Exceptions;
using LearnForge.Domain.Enums;

namespace LearnForge.Domain.Entities;

public sealed class CourseModule : PublishableEntity
{
    private CourseModule()
    {
    }

    internal CourseModule(Guid courseId, string title, int order, string description)
    {
        if (courseId == Guid.Empty)
            throw new DomainException("Course is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Module title is required.");
        // you can do this instead
        // ArgumentException.ThrowIfNullOrWhiteSpace(title);
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Module description is required.");

        if (order < 1)
            throw new DomainException("Module order must be greater than zero.");

        CourseId = courseId;
        Title = title.Trim();
        Description = description.Trim();
        Order = order;
    }

    public Guid CourseId { get; private set; }

    public string Title { get; private set; } = null!;

    public string Description { get; private set; }

    public int Order { get; private set; }

    private readonly List<Lesson> _lessons = [];

    public IReadOnlyList<Lesson> Lessons => _lessons;

    internal void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Module title is required.");

        Title = title.Trim();
    }

    public void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Module description is required.");

        Description = description.Trim();
    }

    internal void ChangeOrder(int order)
    {
        if (order < 1)
            throw new DomainException("Module order must be greater than zero.");

        Order = order;
    }

    protected override void EnsureCanPublish()
    {
        if (_lessons.Count == 0)
            throw new DomainException("Add at least one Lesson before publishing.");
    }

    public Lesson AddLesson(string title, string? description = null, string? content = null)
    {
        var lesson = new Lesson(Id, title, _lessons.Count + 1, description, content);

        _lessons.Add(lesson);

        return lesson;
    }

    public bool RemoveLesson(Guid lessonId)
    {
        if (Status == MaterialStatus.Published && _lessons.Count == 1)
            throw new DomainException("A published module needs at least one lesson. Unpublish it first.");

        var lessonIndex = _lessons.FindIndex(x => x.Id == lessonId);

        if (lessonIndex == -1)
            return false;

        _lessons.RemoveAt(lessonIndex);

        for (var index = lessonIndex; index < _lessons.Count; index++)
        {
            _lessons[index].ChangeOrder(index + 1);
        }

        return true;
    }
}

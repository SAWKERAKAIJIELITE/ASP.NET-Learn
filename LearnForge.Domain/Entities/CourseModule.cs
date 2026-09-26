using LearnForge.Domain.Common;
using LearnForge.Domain.Exceptions;
using LearnForge.Domain.Enums;

namespace LearnForge.Domain.Entities;

public sealed class CourseModule : OrderedEntity
{
    private CourseModule() { }

    internal CourseModule(Guid courseId, string title, int order, string description) : base(title, order)
    {
        if (courseId == Guid.Empty)
            throw new DomainException("Course is required.");

        CourseId = courseId;
        Description = DescriptionValidator.Validate(description, required: true, EntityLabel)!;
    }

    public Guid CourseId { get; private set; }

    public string Description { get; private set; } = null!;

    private readonly List<Lesson> _lessons = [];

    public IReadOnlyList<Lesson> Lessons => _lessons;

    public void ChangeDescription(string description) =>
        Description = DescriptionValidator.Validate(description, required: true, EntityLabel)!;

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
        return Status == MaterialStatus.Published && _lessons.Count == 1
            ? throw new DomainException("A published module needs at least one lesson. Unpublish it first.")
            : OrderedList.Remove(_lessons, lessonId, (l, order) => l.ChangeOrder(order));
    }
}

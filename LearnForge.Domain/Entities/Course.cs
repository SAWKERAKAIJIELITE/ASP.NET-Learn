using LearnForge.Domain.Common;
using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;

namespace LearnForge.Domain.Entities;

public sealed class Course : PublishableEntity
{
    private Course()
    {
    }

    public Course(Guid instructorId, string title, string description)
    {
        if (instructorId == Guid.Empty)
            throw new DomainException("Instructor is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Course title is required.");
        // you can do this instead
        // ArgumentException.ThrowIfNullOrWhiteSpace(title);

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Course Description is required.");

        Title = title.Trim();
        Description = description.Trim();
        InstructorId = instructorId;
    }

    public Guid InstructorId { get; private set; }

    public string Title { get; private set; } = null!;

    public string Description { get; private set; }

    public CourseSettings Settings { get; private set; } = CourseSettings.Default;

    private readonly List<CourseModule> _modules = [];

    public IReadOnlyList<CourseModule> Modules => _modules;

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Course title is required.");

        Title = title.Trim();
    }

    public void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Course Description is required.");

        Description = description.Trim();
    }

    protected override void EnsureCanPublish()
    {
        if (_modules.Count == 0)
            throw new DomainException("Add at least one Module before publishing.");
    }

    public void UpdateSettings(CourseSettings settings) => Settings = settings;

    public CourseModule AddModule(string title, string description)
    {
        var module = new CourseModule(Id, title, _modules.Count + 1, description);

        _modules.Add(module);

        return module;
    }

    public bool RemoveModule(Guid moduleId)
    {
        if (Status == MaterialStatus.Published && _modules.Count == 1)
            throw new DomainException("A published course needs at least one Module. Unpublish it first.");

        var moduleIndex = _modules.FindIndex(x => x.Id == moduleId);

        if (moduleIndex == -1)
            return false;

        _modules.RemoveAt(moduleIndex);

        for (var index = moduleIndex; index < _modules.Count; index++)
        {
            _modules[index].ChangeOrder(index + 1);
        }

        return true;
    }
}
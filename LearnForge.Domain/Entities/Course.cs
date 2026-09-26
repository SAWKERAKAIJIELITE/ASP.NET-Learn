using LearnForge.Domain.Common;
using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;

namespace LearnForge.Domain.Entities;

public sealed class Course : Entity
{
    private Course() { }

    public Course(Guid instructorId, string title, string description) : base(title)
    {
        if (instructorId == Guid.Empty)
            throw new DomainException("Instructor is required.");

        Description = DescriptionValidator.Validate(description, required: true, EntityLabel)!;
        InstructorId = instructorId;
    }

    public Guid InstructorId { get; private set; }

    public string Description { get; private set; } = null!;

    public CourseSettings Settings { get; private set; } = CourseSettings.Default;

    private readonly List<CourseModule> _modules = [];

    public IReadOnlyList<CourseModule> Modules => _modules;

    public void ChangeDescription(string description) =>
        Description = DescriptionValidator.Validate(description, required: true, EntityLabel)!;

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
        return Status == MaterialStatus.Published && _modules.Count == 1
            ? throw new DomainException("A published course needs at least one Module. Unpublish it first.")
            : OrderedList.Remove(_modules, moduleId, (l, order) => l.ChangeOrder(order));
    }
}
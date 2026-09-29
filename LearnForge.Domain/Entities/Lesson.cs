using LearnForge.Domain.Common;
using LearnForge.Domain.Exceptions;
using LearnForge.Domain.Enums;

namespace LearnForge.Domain.Entities;

public sealed class Lesson : OrderedEntity
{
    private Lesson() { }

    internal Lesson(
        Guid courseModuleId,
        string title,
        int order,
        string? description = null,
        string? contentMarkdown = null
    ) : base(title, order)
    {
        if (courseModuleId == Guid.Empty)
            throw new DomainException("Course module is required.");

        // if (string.IsNullOrWhiteSpace(contentMarkdown))
        //     throw new DomainException("Lesson content is required.");

        CourseModuleId = courseModuleId;
        Description = DescriptionValidator.Validate(description, required: false, EntityLabel)!;
        ContentMarkdown = contentMarkdown?.Trim();
    }

    public Guid CourseModuleId { get; private set; }

    public string? Description { get; private set; }

    public string? ContentMarkdown { get; private set; }

    private readonly List<Activity> _activities = [];

    public IReadOnlyList<Activity> Activities => _activities;

    private readonly List<LessonResource> _resources = [];

    public IReadOnlyList<LessonResource> Resources => _resources;

    public void ChangeDescription(string? description = null) =>
        Description = DescriptionValidator.Validate(description, required: false, EntityLabel)!;

    public void UpdateContent(string? markdown = null)
    {
        if (Status == MaterialStatus.Published && string.IsNullOrWhiteSpace(markdown))
            throw new DomainException("A published Lesson can't have empty content. Unpublish it first.");

        ContentMarkdown = markdown?.Trim();
    }

    protected override void EnsureCanPublish()
    {
        if (string.IsNullOrWhiteSpace(ContentMarkdown))
            throw new DomainException("Content Must be Provided before publishing.");
    }

    public Activity AddActivity(ActivityType type, string title, string prompt, string contentJson)
    {
        var activity = new Activity(Id, type, title, _activities.Count + 1, prompt, contentJson);

        _activities.Add(activity);

        return activity;
    }

    public bool RemoveActivity(Guid activityId)
    {
        return OrderedList.Remove(_activities, activityId, (l, order) => l.ChangeOrder(order));
    }

    public LessonResource AddResource(LessonResourceType type, string title, string url)
    {
        var resource = new LessonResource(Id, title, url, _resources.Count + 1, type);

        _resources.Add(resource);

        return resource;
    }

    public bool RemoveResource(Guid resourceId)
    {
        return OrderedList.Remove(_resources, resourceId, (l, order) => l.ChangeOrder(order));
    }
}
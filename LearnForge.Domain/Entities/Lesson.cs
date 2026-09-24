using LearnForge.Domain.Common;
using LearnForge.Domain.Exceptions;
using LearnForge.Domain.Enums;

namespace LearnForge.Domain.Entities;

public sealed class Lesson : PublishableEntity
{
    private Lesson()
    {
    }

    internal Lesson(
        Guid courseModuleId,
        string title,
        int order,
        string? description = null,
        string? contentMarkdown = null
    )
    {
        if (courseModuleId == Guid.Empty)
            throw new DomainException("Course module is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Lesson title is required.");

        // if (string.IsNullOrWhiteSpace(contentMarkdown))
        //     throw new DomainException("Lesson content is required.");

        if (order < 1)
            throw new DomainException("Lesson order must be greater than zero.");

        CourseModuleId = courseModuleId;
        Title = title.Trim();
        Order = order;
        Description = description?.Trim();
        ContentMarkdown = contentMarkdown?.Trim();
    }

    public Guid CourseModuleId { get; private set; }

    public string Title { get; private set; } = null!;

    public int Order { get; private set; }

    public string? Description { get; private set; }

    public string? ContentMarkdown { get; private set; }

    private readonly List<Activity> _activities = [];

    public IReadOnlyList<Activity> Activities => _activities;

    private readonly List<LessonResource> _resources = [];

    public IReadOnlyList<LessonResource> Resources => _resources;

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Lesson title is required.");

        Title = title.Trim();
    }

    public void ChangeDescription(string? description = null)
    {
        Description = description?.Trim();
    }

    public void UpdateContent(string? markdown=null)
    {
        if (Status == MaterialStatus.Published && string.IsNullOrWhiteSpace(markdown))
            throw new DomainException("A published Lesson can't have empty content. Unpublish it first.");

        ContentMarkdown = markdown?.Trim();
    }

    public void ChangeOrder(int order)
    {
        if (order < 1)
            throw new DomainException("Lesson order must be greater than zero.");

        Order = order;
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
        var activityIndex = _activities.FindIndex(x => x.Id == activityId);

        if (activityIndex == -1)
            return false;

        _activities.RemoveAt(activityIndex);

        for (var index = activityIndex; index < _activities.Count; index++)
        {
            _activities[index].ChangeOrder(index + 1);
        }

        return true;
    }

    public LessonResource AddResource(LessonResourceType type, string title, string url)
    {
        var resource = new LessonResource(Id, title, url, _resources.Count + 1, type);

        _resources.Add(resource);

        return resource;
    }

    public bool RemoveResource(Guid resourceId)
    {
        var resourceIndex = _resources.FindIndex(x => x.Id == resourceId);

        if (resourceIndex == -1)
            return false;

        _resources.RemoveAt(resourceIndex);

        for (var index = resourceIndex; index < _resources.Count; index++)
        {
            _resources[index].ChangeOrder(index + 1);
        }

        return true;
    }
}
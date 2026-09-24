using LearnForge.Domain.Entities;
using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;

namespace LearnForge.UnitTests.Domain;

public class LessonActivityTests
{
    [Fact]
    public void AddActivity_WithValidData_AddsActivity()
    {
        var lesson = CreateLesson();

        var activity = lesson.AddActivity(ActivityType.Quiz, "Variables Quiz");

        Assert.Single(lesson.Activities);
        Assert.Equal("Variables Quiz", activity.Title);
        Assert.Equal(ActivityType.Quiz, activity.Type);
        Assert.Equal(lesson.Id, activity.LessonId);
        Assert.Equal(1, activity.Order);
    }

    [Fact]
    public void AddActivity_MultipleTimes_AssignsSequentialOrder()
    {
        var lesson = CreateLesson();

        var first = lesson.AddActivity(ActivityType.Article, "Introduction");

        var second = lesson.AddActivity(ActivityType.Quiz, "Variables Quiz");

        var third = lesson.AddActivity(ActivityType.CodeChallenge, "Fix the Code");

        Assert.Equal(1, first.Order);
        Assert.Equal(2, second.Order);
        Assert.Equal(3, third.Order);
    }

    [Fact]
    public void AddActivity_WithEmptyTitle_ThrowsDomainException()
    {
        var lesson = CreateLesson();

        var exception = Assert.Throws<DomainException>(() => lesson.AddActivity(ActivityType.Quiz, "   "));

        Assert.Equal("Activity title is required.", exception.Message);
        Assert.Empty(lesson.Activities);
    }

    [Fact]
    public void RemoveActivity_WithExistingActivity_RemovesIt()
    {
        var lesson = CreateLesson();

        var first = lesson.AddActivity(ActivityType.Article, "Introduction");

        var second = lesson.AddActivity(ActivityType.Quiz, "Quiz");

        var removed = lesson.RemoveActivity(first.Id);

        Assert.True(removed);
        Assert.Single(lesson.Activities);
        Assert.Equal(second.Id, lesson.Activities[0].Id);
    }

    [Fact]
    public void RemoveActivity_ReordersRemainingActivities()
    {
        var lesson = CreateLesson();

        var first = lesson.AddActivity(ActivityType.Article, "Introduction");

        lesson.AddActivity(ActivityType.Quiz, "Quiz");

        var third = lesson.AddActivity(ActivityType.CodeChallenge, "Challenge");

        lesson.RemoveActivity(first.Id);

        Assert.Equal(1, lesson.Activities[0].Order);
        Assert.Equal(2, lesson.Activities[1].Order);
        Assert.Equal(third.Id, lesson.Activities[1].Id);
    }

    [Fact]
    public void RemoveActivity_WithUnknownId_ReturnsFalse()
    {
        var lesson = CreateLesson();

        lesson.AddActivity(ActivityType.Quiz, "Quiz");

        var removed = lesson.RemoveActivity(Guid.NewGuid());

        Assert.False(removed);
        Assert.Single(lesson.Activities);
    }

    private static Lesson CreateLesson()
    {
        var course = new Course(Guid.NewGuid(), "C# Fundamentals", null);

        var module = course.AddModule("Fundamentals");

        return module.AddLesson("Variables");
    }
}
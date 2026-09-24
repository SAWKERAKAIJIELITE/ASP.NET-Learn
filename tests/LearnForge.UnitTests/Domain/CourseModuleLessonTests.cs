using LearnForge.Domain.Entities;
using LearnForge.Domain.Exceptions;

namespace LearnForge.UnitTests.Domain;

public class CourseModuleLessonTests
{
    [Fact]
    public void AddLesson_WithValidTitle_AddsLesson()
    {
        var module = CreateModule();

        var lesson = module.AddLesson("  Variables  ");

        Assert.Single(module.Lessons);
        Assert.Equal("Variables", lesson.Title);
        Assert.Equal(module.Id, lesson.CourseModuleId);
        Assert.Equal(1, lesson.Order);
    }

    [Fact]
    public void AddLesson_MultipleTimes_AssignsSequentialOrder()
    {
        var module = CreateModule();

        var first = module.AddLesson("Variables");
        var second = module.AddLesson("Types");
        var third = module.AddLesson("Methods");

        Assert.Equal(1, first.Order);
        Assert.Equal(2, second.Order);
        Assert.Equal(3, third.Order);
    }

    [Fact]
    public void AddLesson_WithEmptyTitle_ThrowsDomainException()
    {
        var module = CreateModule();

        var exception = Assert.Throws<DomainException>(() => module.AddLesson("   "));

        Assert.Equal("Lesson title is required.", exception.Message);
        Assert.Empty(module.Lessons);
    }

    [Fact]
    public void RemoveLesson_WithExistingLesson_RemovesIt()
    {
        var module = CreateModule();

        var first = module.AddLesson("Variables");
        var second = module.AddLesson("Types");

        var removed = module.RemoveLesson(first.Id);

        Assert.True(removed);
        Assert.Single(module.Lessons);
        Assert.Equal(second.Id, module.Lessons[0].Id);
    }

    [Fact]
    public void RemoveLesson_ReordersRemainingLessons()
    {
        var module = CreateModule();

        var first = module.AddLesson("Variables");
        module.AddLesson("Types");
        var third = module.AddLesson("Methods");

        module.RemoveLesson(first.Id);

        Assert.Equal(1, module.Lessons[0].Order);
        Assert.Equal(2, module.Lessons[1].Order);
        Assert.Equal(third.Id, module.Lessons[1].Id);
    }

    [Fact]
    public void RemoveLesson_WithUnknownId_ReturnsFalse()
    {
        var module = CreateModule();

        module.AddLesson("Variables");

        var removed = module.RemoveLesson(Guid.NewGuid());

        Assert.False(removed);
        Assert.Single(module.Lessons);
    }

    private static CourseModule CreateModule()
    {
        var course = new Course(Guid.NewGuid(), "C# Fundamentals", null);

        return course.AddModule("Fundamentals");
    }
}
using LearnForge.Domain.Entities;
using LearnForge.Domain.Exceptions;

namespace LearnForge.UnitTests.Domain;

public class CourseModuleTests
{
    [Fact]
    public void AddModule_WithValidTitle_AddsModule()
    {
        var course = CreateCourse();

        var module = course.AddModule("  Fundamentals  ");

        Assert.Single(course.Modules);
        Assert.Equal("Fundamentals", module.Title);
        Assert.Equal(course.Id, module.CourseId);
        Assert.Equal(1, module.Order);
    }

    [Fact]
    public void AddModule_MultipleTimes_AssignsSequentialOrder()
    {
        var course = CreateCourse();

        var first = course.AddModule("Fundamentals");
        var second = course.AddModule("OOP");
        var third = course.AddModule("LINQ");

        Assert.Equal(1, first.Order);
        Assert.Equal(2, second.Order);
        Assert.Equal(3, third.Order);
    }

    [Fact]
    public void AddModule_WithEmptyTitle_ThrowsDomainException()
    {
        var course = CreateCourse();

        var exception = Assert.Throws<DomainException>(() => course.AddModule("   "));

        Assert.Equal("Module title is required.", exception.Message);
        Assert.Empty(course.Modules);
    }

    [Fact]
    public void RemoveModule_WithExistingModule_RemovesIt()
    {
        var course = CreateCourse();

        var first = course.AddModule("Fundamentals");
        var second = course.AddModule("OOP");

        var removed = course.RemoveModule(first.Id);

        Assert.True(removed);
        Assert.Single(course.Modules);
        Assert.Equal(second.Id, course.Modules[0].Id);
    }

    [Fact]
    public void RemoveModule_ReordersRemainingModules()
    {
        var course = CreateCourse();

        var first = course.AddModule("Fundamentals");
        course.AddModule("OOP");
        var third = course.AddModule("LINQ");

        course.RemoveModule(first.Id);

        Assert.Equal(1, course.Modules[0].Order);
        Assert.Equal(2, course.Modules[1].Order);
        Assert.Equal(third.Id, course.Modules[1].Id);
    }

    [Fact]
    public void RemoveModule_WithUnknownId_ReturnsFalse()
    {
        var course = CreateCourse();

        course.AddModule("Fundamentals");

        var removed = course.RemoveModule(Guid.NewGuid());

        Assert.False(removed);
        Assert.Single(course.Modules);
    }

    private static Course CreateCourse()
    {
        return new Course(Guid.NewGuid(), "C# Fundamentals", "Learn C#");
    }
}
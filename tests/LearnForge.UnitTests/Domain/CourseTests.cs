using LearnForge.Domain.Entities;
using LearnForge.Domain.Enums;
using LearnForge.Domain.Exceptions;

namespace LearnForge.UnitTests.Domain;

public class CourseTests
{
    private readonly Guid _instructorId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithValidData_CreatesDraftCourse()
    {
        var course = new Course(
            _instructorId,
            "  C# Fundamentals  ",
            "  Learn modern C#  "
        );

        Assert.NotEqual(Guid.Empty, course.Id);
        Assert.Equal(_instructorId, course.InstructorId);
        Assert.Equal("C# Fundamentals", course.Title);
        Assert.Equal("Learn modern C#", course.Description);
        Assert.Equal(MaterialStatus.Draft, course.Status);
        Assert.NotEqual(default, course.CreatedAt);
        Assert.Null(course.UpdatedAt);
        Assert.Null(course.DeletedAt);
    }

    [Fact]
    public void Constructor_WithEmptyInstructorId_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Course(
                Guid.Empty,
                "C# Fundamentals",
                null
            )
        );

        Assert.Equal("Instructor is required.", exception.Message);
    }

    [Fact]
    public void Constructor_WithEmptyTitle_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Course(
                _instructorId,
                "   ",
                null
            )
        );

        Assert.Equal("Course title is required.", exception.Message);
    }

    [Fact]
    public void Rename_WithValidTitle_UpdatesTitle()
    {
        var course = CreateCourse();

        course.Rename("  Advanced C#  ");

        Assert.Equal("Advanced C#", course.Title);
    }

    [Fact]
    public void Rename_WithEmptyTitle_ThrowsDomainException()
    {
        var course = CreateCourse();

        var exception = Assert.Throws<DomainException>(() => course.Rename("   "));

        Assert.Equal("Course title is required.", exception.Message);
    }

    [Fact]
    public void ChangeDescription_WithValue_UpdatesDescription()
    {
        var course = CreateCourse();

        course.ChangeDescription("  New description  ");

        Assert.Equal("New description", course.Description);
    }

    [Fact]
    public void ChangeDescription_WithNull_ClearsDescription()
    {
        var course = CreateCourse();

        course.ChangeDescription(null);

        Assert.Null(course.Description);
    }

    [Fact]
    public void Publish_ChangesStatusToPublished()
    {
        var course = CreateCourse();

        course.Publish();

        Assert.Equal(MaterialStatus.Published, course.Status);
    }

    [Fact]
    public void Publish_WhenAlreadyPublished_DoesNothing()
    {
        var course = CreateCourse();

        course.Publish();
        course.Publish();

        Assert.Equal(MaterialStatus.Published, course.Status);
    }

    [Fact]
    public void Unpublish_ChangesPublishedCourseToDraft()
    {
        var course = CreateCourse();

        course.Publish();
        course.Unpublish();

        Assert.Equal(MaterialStatus.Draft, course.Status);
    }

    private Course CreateCourse()
    {
        return new Course(_instructorId, "C# Fundamentals", "Learn C#");
    }
}
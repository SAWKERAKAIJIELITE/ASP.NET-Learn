namespace LearnForge.Application.Courses.CreateCourse;

public sealed record CreateCourseCommand(Guid InstructorId, string Title, string Description);
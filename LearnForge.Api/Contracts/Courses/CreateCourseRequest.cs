namespace LearnForge.Api.Contracts.Courses;

public sealed record CreateCourseRequest(Guid InstructorId, string Title, string Description);
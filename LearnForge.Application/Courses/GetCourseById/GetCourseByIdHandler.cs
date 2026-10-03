using LearnForge.Application.Common.Interfaces;

namespace LearnForge.Application.Courses.GetCourseById;

public sealed class GetCourseByIdHandler(ICourseRepository repository)
{
    public async Task<CourseResult?> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
    {
        var course = await repository.GetByIdAsync(query.Id, cancellationToken);
        return course is null ? null : new CourseResult(course.Id, course.Title, course.Description);
    }
}
using LearnForge.Application.Common.Interfaces;
using LearnForge.Domain.Entities;

namespace LearnForge.Application.Courses.CreateCourse;

public sealed class CreateCourseHandler(ICourseRepository courseRepository)
{
    public async Task<CreateCourseResult> HandleAsync(
        CreateCourseCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var course = new Course(command.InstructorId, command.Title, command.Description);

        await courseRepository.AddAsync(course, cancellationToken);

        await courseRepository.SaveChangesAsync(cancellationToken);

        return new CreateCourseResult(course.Id);
    }
}
using LearnForge.Application.Common.Interfaces;
using LearnForge.Domain.Entities;

namespace LearnForge.Application.Courses.CreateCourse;

public sealed class CreateCourseHandler(ICourseRepository repository)
{
    public async Task<CourseResult> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        var course = new Course(command.InstructorId, command.Title, command.Description);

        await repository.AddAsync(course, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CourseResult(course.Id, course.Title, course.Description);
    }
}
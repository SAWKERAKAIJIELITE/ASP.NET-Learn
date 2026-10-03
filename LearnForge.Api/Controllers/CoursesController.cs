using LearnForge.Api.Contracts.Courses;
using LearnForge.Application.Courses;
using LearnForge.Application.Courses.CreateCourse;
using LearnForge.Application.Courses.GetCourseById;

using Microsoft.AspNetCore.Mvc;

namespace LearnForge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CoursesController(
    CreateCourseHandler createCourseHandler,
    GetCourseByIdHandler getCourseByIdHandler
) : ControllerBase
{
    [HttpPost(Name = "CreateNewCourse")]
    public async Task<ActionResult<CourseResult>> Create(
        CreateCourseRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateCourseCommand(request.InstructorId, request.Title, request.Description);
        var result = await createCourseHandler.Handle(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await getCourseByIdHandler.Handle(new GetCourseByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
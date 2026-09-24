using LearnForge.Api.Contracts.Courses;
using LearnForge.Application.Courses.CreateCourse;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnForge.API.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public sealed class CoursesController(CreateCourseHandler createCourseHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateCourseResponse>> Create(
            CreateCourseRequest request,
            CancellationToken cancellationToken
        )
        {
            var command = new CreateCourseCommand(request.InstructorId,request.Title,request.Description);

            var result = await createCourseHandler.HandleAsync(command,cancellationToken);

            var response = new CreateCourseResponse(result.CourseId);

            return Created($"/api/courses/{response.Id}",response);
        }
    }
}

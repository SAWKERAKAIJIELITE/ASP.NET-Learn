using LearnForge.Application.Common.Interfaces;
using LearnForge.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace LearnForge.Infrastructure.Persistence.Repositories;

public sealed class CourseRepository(LearnForgeDbContext context) : ICourseRepository
{
    public async Task AddAsync(Course course, CancellationToken cancellationToken)
    {
        await context.Courses.AddAsync(course, cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Courses
            .Include(x => x.Modules)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
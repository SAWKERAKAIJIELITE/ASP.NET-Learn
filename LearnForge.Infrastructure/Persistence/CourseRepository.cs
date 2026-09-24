using LearnForge.Application.Common.Interfaces;
using LearnForge.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace LearnForge.Infrastructure.Persistence;

public sealed class CourseRepository(LearnForgeDbContext dbContext) : ICourseRepository
{
    public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        await dbContext.Courses.AddAsync(course, cancellationToken);
    }

    public Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Courses
            .Include(x => x.Modules)
            .ThenInclude(x => x.Lessons)
            .ThenInclude(x => x.Activities)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
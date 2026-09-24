using LearnForge.Domain.Entities;

namespace LearnForge.Application.Common.Interfaces;

public interface ICourseRepository
{
    Task AddAsync(Course course, CancellationToken cancellationToken = default);

    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
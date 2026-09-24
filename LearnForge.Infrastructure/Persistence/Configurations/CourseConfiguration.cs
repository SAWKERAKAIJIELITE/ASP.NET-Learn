using LearnForge.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;


public sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

        builder.Property(x => x.Description).HasMaxLength(2000);

        builder.Property(x => x.Status).IsRequired();

        builder.Property(x => x.InstructorId).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.DeletedAt);

        builder.HasMany(x => x.Modules)
            .WithOne()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.InstructorId);
    }
}
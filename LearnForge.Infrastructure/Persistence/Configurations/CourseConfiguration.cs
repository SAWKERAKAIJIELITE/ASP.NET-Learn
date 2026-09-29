using LearnForge.Domain.Entities;
using LearnForge.Infrastructure.Persistence.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;


public sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");

        builder.ConfigureEntity();

        builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);

        builder.ComplexProperty(x => x.Settings);

        builder.Property(x => x.InstructorId).IsRequired();

        builder.HasMany(x => x.Modules)
            .WithOne()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(x => x.InstructorId);
    }
}
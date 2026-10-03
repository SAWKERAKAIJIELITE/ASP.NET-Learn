using LearnForge.Domain.Enums;
using LearnForge.Infrastructure.Persistence.ReferenceData;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnForge.Infrastructure.Persistence.Configurations;

public sealed class MaterialStatusLookupConfiguration : IEntityTypeConfiguration<MaterialStatusLookup>
{
    public void Configure(EntityTypeBuilder<MaterialStatusLookup> builder)
    {
        builder.ToTable("material_statuses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(20);
        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasData(
            new MaterialStatusLookup { Id = MaterialStatus.Draft, Code = nameof(MaterialStatus.Draft) },
            new MaterialStatusLookup { Id = MaterialStatus.Published, Code = nameof(MaterialStatus.Published) }
        );
    }
}
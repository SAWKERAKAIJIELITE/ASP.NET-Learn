using LearnForge.Domain.Common;
using LearnForge.Domain.Enums;
using LearnForge.Infrastructure.Persistence.ReferenceData;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace LearnForge.Infrastructure.Persistence.Extensions;

public static class EntityTypeBuilderExtension
{
    public static void ConfigureBaseEntity<T>(this EntityTypeBuilder<T> builder) where T : BaseEntity
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        builder.Property(x => x.DeletedAt).HasColumnType("timestamptz");

        builder.HasIndex(x => x.DeletedAt);

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }

    public static void ConfigureEntity<T>(this EntityTypeBuilder<T> builder) where T : Entity
    {
        builder.ConfigureBaseEntity();

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        // builder.Property(x => x.Status)
        //     // .HasColumnType("material_status")
        //     // .HasDefaultValue(MaterialStatus.Draft)
        //     // .HasConversion<string>()
        //     // .HasConversion(
        //     //     v => v.ToString().ToLower(),
        //     //     v => Enum.Parse<MaterialStatus>(v, true)
        //     // )
        //     // .HasDefaultValueSql("'draft'::material_status")
        //     .HasSentinel(default);

        builder.Property(x => x.Status)
            .HasDefaultValue(MaterialStatus.Draft)
            .HasSentinel(default);

        builder.HasOne<MaterialStatusLookup>()
            .WithMany()
            .HasForeignKey(x => x.Status)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(t =>
            t.HasCheckConstraint($"CK_{typeof(T).Name}_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0")
        );
    }

    public static void ConfigureOrderedEntity<T>(this EntityTypeBuilder<T> builder) where T : OrderedEntity
    {
        builder.ConfigureEntity();
        builder.Property(x => x.Order).IsRequired();

        builder.ToTable(t =>
            t.HasCheckConstraint($"CK_{typeof(T).Name}_Order_Positive", "\"Order\" >= 1")
        );
    }
}
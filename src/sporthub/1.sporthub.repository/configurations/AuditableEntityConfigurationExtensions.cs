using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sporthub.domain;

namespace sporthub.repository;

internal static class AuditableEntityConfigurationExtensions
{
    internal static EntityTypeBuilder<TEntity> ConfigureAuditableEntity<TEntity>(
        this EntityTypeBuilder<TEntity> builder)
        where TEntity : AuditableEntity
    {
        builder.HasKey(x => x.id);
        builder.Property(x => x.id).ValueGeneratedOnAdd();
        builder.Property(x => x.created_date).IsRequired();
        builder.HasQueryFilter(x => x.deleted_date == null);

        return builder;
    }
}

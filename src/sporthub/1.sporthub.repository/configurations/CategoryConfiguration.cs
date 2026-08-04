using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportHub.Domain;

namespace sporthub.repository
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(x => x.id);

            builder.Property(x => x.name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.slug)
                .HasMaxLength(220)
                .IsRequired();

            builder.HasIndex(x => x.slug)
                .IsUnique();

            builder.Property(x => x.description)
                .HasMaxLength(1000);

            builder.Property(x => x.image_url)
                .HasMaxLength(500);

            builder.HasOne(x => x.parent)
                .WithMany(x => x.children)
                .HasForeignKey(x => x.parent_id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
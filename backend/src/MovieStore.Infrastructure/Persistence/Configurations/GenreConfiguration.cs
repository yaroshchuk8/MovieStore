using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Configurations;

internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).IsRequired();
        builder.Property(g => g.Description).IsRequired(false);
        builder.Property(g => g.CreatedAt).IsRequired();

        // Many-to-Many Relationship
        // builder.HasMany(g => g.Movies)
        //     .WithMany(m => m.Genres)
        //     .UsingEntity(j =>
        //     {
        //         j.ToTable("MovieGenres");
        //         j.Property("MoviesId").HasColumnName("MovieId");
        //         j.Property("GenresId").HasColumnName("GenreId");
        //     });
    }
}
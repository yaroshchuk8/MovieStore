using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Genres;
using MovieStore.Infrastructure.Common.Persistence.Constants;

namespace MovieStore.Infrastructure.Genres.Persistence.Configurations;

internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).HasMaxLength(Genre.NameMaxLength).IsRequired();
        builder.Property(g => g.Description).HasMaxLength(Genre.DescriptionMaxLength).IsRequired(false);
        builder.Property(g => g.CreatedAt).HasDefaultValueSql(SqlConstants.UtcDate).IsRequired();

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
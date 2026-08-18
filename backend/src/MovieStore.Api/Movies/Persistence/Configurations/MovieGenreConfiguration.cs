using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Movies;

namespace MovieStore.Infrastructure.Movies.Persistence.Configurations;

public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.HasKey(mg => new { mg.MovieId, mg.GenreId });
    }
}
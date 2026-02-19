using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Movies;
using MovieStore.Infrastructure.Common.Persistence.Constants;

namespace MovieStore.Infrastructure.Movies.Persistence.Configurations;

internal class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title).HasMaxLength(Movie.TitleMaxLength).IsRequired();
        builder.Property(m => m.Description).HasMaxLength(Movie.DescriptionMaxLength).IsRequired();
        builder.Property(m => m.Price).HasPrecision(Movie.PricePrecision, Movie.PriceScale).IsRequired();
        builder.Property(m => m.CreatedAt).HasDefaultValueSql(SqlConstants.UtcDate).IsRequired();
        builder.Property(m => m.PosterKey).HasMaxLength(Movie.PosterKeyMaxLength).IsRequired();
        
        builder.HasIndex(m => m.PublisherProfileId).IsUnique();
    }
}
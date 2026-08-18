using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Common.Constants;
using MovieStore.Domain.Movies;

namespace MovieStore.Infrastructure.Movies.Persistence.Configurations;

public class MoviePrivateAssetConfiguration : IEntityTypeConfiguration<MoviePrivateAsset>
{
    public void Configure(EntityTypeBuilder<MoviePrivateAsset> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Key).HasMaxLength(FileConstants.FileKeyMaxLength).IsRequired();
    }
}
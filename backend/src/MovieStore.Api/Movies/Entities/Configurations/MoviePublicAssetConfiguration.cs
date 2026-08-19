using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Api.Common.FileStorage;

namespace MovieStore.Api.Movies.Entities.Configurations;

public class MoviePublicAssetConfiguration : IEntityTypeConfiguration<MoviePublicAsset>
{
    public void Configure(EntityTypeBuilder<MoviePublicAsset> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Key).HasMaxLength(FileConstants.FileKeyMaxLength).IsRequired();
    }
}
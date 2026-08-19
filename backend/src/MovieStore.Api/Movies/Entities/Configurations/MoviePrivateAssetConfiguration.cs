using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Api.Common.FileStorage;

namespace MovieStore.Api.Movies.Entities.Configurations;

public class MoviePrivateAssetConfiguration : IEntityTypeConfiguration<MoviePrivateAsset>
{
    public void Configure(EntityTypeBuilder<MoviePrivateAsset> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Key).HasMaxLength(FileConstants.FileKeyMaxLength).IsRequired();
    }
}
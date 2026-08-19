using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Api.Common.Persistence.Constants;

namespace MovieStore.Api.Users.Entities.Domain.Configurations;

public class PublisherProfileConfiguration : IEntityTypeConfiguration<PublisherProfile>
{
    public void Configure(EntityTypeBuilder<PublisherProfile> builder)
    {
        builder.Property(pp => pp.StudioName).HasMaxLength(PublisherProfile.StudioNameMaxLength).IsRequired();
        builder.Property(pp => pp.CreatedAt).HasDefaultValueSql(SqlConstants.UtcDate).IsRequired();
        builder.HasIndex(w => w.UserProfileId).IsUnique();
    }
}
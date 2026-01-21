using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Users;
using MovieStore.Infrastructure.Common.Persistence.Constants;

namespace MovieStore.Infrastructure.Users.Persistence.Configurations;

public class PublisherProfileConfiguration : IEntityTypeConfiguration<PublisherProfile>
{
    public void Configure(EntityTypeBuilder<PublisherProfile> builder)
    {
        builder.Property(pp => pp.StudioName).HasMaxLength(PublisherProfile.StudioNameMaxLength).IsRequired();
        builder.Property(pp => pp.CreatedAt).HasDefaultValueSql(SqlConstants.UtcDate).IsRequired();
        builder.HasIndex(w => w.UserProfileId).IsUnique();
    }
}
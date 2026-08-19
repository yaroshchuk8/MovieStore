using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Api.Common.Persistence.Constants;
using MovieStore.Api.Users.Entities.Identity;

namespace MovieStore.Api.Users.Entities.Domain.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(up => up.Id);
        builder.Property(up => up.Name).HasMaxLength(UserProfile.NameMaxLength).IsRequired(false);
        builder.Property(up => up.Sex).IsRequired(false);
        builder.Property(up => up.CreatedAt).HasDefaultValueSql(SqlConstants.UtcDate).IsRequired();
        
        builder
            .HasOne<IdentityUserEntity>()
            .WithOne()
            .HasForeignKey<UserProfile>(profile => profile.IdentityUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(p => p.IdentityUserId).IsUnique();
    }
}
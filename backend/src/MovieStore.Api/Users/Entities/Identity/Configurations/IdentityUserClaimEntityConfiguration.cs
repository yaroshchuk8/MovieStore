using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Users.Entities.Identity.Configurations;

public class IdentityUserClaimEntityConfiguration : IEntityTypeConfiguration<IdentityUserClaimEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaimEntity> builder)
    {
        builder.ToTable("IdentityUserClaim");
    }
}
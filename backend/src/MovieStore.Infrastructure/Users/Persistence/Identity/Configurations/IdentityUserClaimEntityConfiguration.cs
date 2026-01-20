using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Configurations;

public class IdentityUserClaimEntityConfiguration : IEntityTypeConfiguration<IdentityUserClaimEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaimEntity> builder)
    {
        builder.ToTable("IdentityUserClaim");
    }
}
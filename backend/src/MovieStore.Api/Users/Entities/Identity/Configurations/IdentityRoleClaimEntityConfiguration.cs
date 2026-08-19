using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Users.Entities.Identity.Configurations;

public class IdentityRoleClaimEntityConfiguration : IEntityTypeConfiguration<IdentityRoleClaimEntity>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaimEntity> builder)
    {
        builder.ToTable("IdentityRoleClaim");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Configurations;

public class IdentityRoleClaimEntityConfiguration : IEntityTypeConfiguration<IdentityRoleClaimEntity>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaimEntity> builder)
    {
        builder.ToTable("IdentityRoleClaim");
    }
}
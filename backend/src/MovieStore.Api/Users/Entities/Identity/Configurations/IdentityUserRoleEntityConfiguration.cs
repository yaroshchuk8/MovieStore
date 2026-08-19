using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Users.Entities.Identity.Configurations;

public class IdentityUserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRoleEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserRoleEntity> builder)
    {
        builder.ToTable("IdentityUserRole");
    }
}
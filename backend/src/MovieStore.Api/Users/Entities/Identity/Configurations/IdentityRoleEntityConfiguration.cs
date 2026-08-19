using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Users.Entities.Identity.Configurations;

public class IdentityRoleEntityConfiguration : IEntityTypeConfiguration<IdentityRoleEntity>
{
    public void Configure(EntityTypeBuilder<IdentityRoleEntity> builder)
    {
        builder.ToTable("IdentityRole");
    }
}
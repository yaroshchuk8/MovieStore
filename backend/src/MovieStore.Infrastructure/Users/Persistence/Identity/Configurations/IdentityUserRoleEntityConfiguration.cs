using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Configurations;

public class IdentityUserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRoleEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserRoleEntity> builder)
    {
        builder.ToTable("IdentityUserRole");
    }
}
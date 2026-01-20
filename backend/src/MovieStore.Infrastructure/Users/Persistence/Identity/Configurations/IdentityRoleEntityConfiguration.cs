using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Configurations;

public class IdentityRoleEntityConfiguration : IEntityTypeConfiguration<IdentityRoleEntity>
{
    public void Configure(EntityTypeBuilder<IdentityRoleEntity> builder)
    {
        builder.ToTable("IdentityRole");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Configurations;

public class IdentityUserEntityConfiguration : IEntityTypeConfiguration<IdentityUserEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserEntity> builder)
    {
        builder.ToTable("IdentityUser");
    }
}
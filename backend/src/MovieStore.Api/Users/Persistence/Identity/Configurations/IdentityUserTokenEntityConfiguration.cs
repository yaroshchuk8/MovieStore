using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Configurations;

public class IdentityUserTokenEntityConfiguration : IEntityTypeConfiguration<IdentityUserTokenEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserTokenEntity> builder)
    {
        builder.ToTable("IdentityUserToken");
    }
}
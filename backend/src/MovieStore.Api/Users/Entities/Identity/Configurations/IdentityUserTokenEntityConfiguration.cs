using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Users.Entities.Identity.Configurations;

public class IdentityUserTokenEntityConfiguration : IEntityTypeConfiguration<IdentityUserTokenEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserTokenEntity> builder)
    {
        builder.ToTable("IdentityUserToken");
    }
}
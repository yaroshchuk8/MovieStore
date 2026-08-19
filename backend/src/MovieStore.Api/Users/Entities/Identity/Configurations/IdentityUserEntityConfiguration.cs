using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Users.Entities.Identity.Configurations;

public class IdentityUserEntityConfiguration : IEntityTypeConfiguration<IdentityUserEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserEntity> builder)
    {
        builder.ToTable("IdentityUser");
    }
}
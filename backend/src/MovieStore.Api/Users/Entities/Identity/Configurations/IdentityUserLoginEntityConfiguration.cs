using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Users.Entities.Identity.Configurations;

public class IdentityUserLoginEntityConfiguration : IEntityTypeConfiguration<IdentityUserLoginEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserLoginEntity> builder)
    {
        builder.ToTable("IdentityUserLogin");
    }
}
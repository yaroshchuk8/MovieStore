using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Configurations;

public class IdentityUserLoginEntityConfiguration : IEntityTypeConfiguration<IdentityUserLoginEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserLoginEntity> builder)
    {
        builder.ToTable("IdentityUserLogin");
    }
}
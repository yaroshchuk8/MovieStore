using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Users.Entities.Domain.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.Property(w => w.ConcurrencyToken).IsRowVersion();
        builder.Property(w => w.Balance).HasPrecision(Wallet.BalancePrecision, Wallet.BalanceScale).IsRequired();
        
        builder.HasIndex(w => w.UserProfileId).IsUnique();
    }
}
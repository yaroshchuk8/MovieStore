using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Users;

namespace MovieStore.Infrastructure.Users.Persistence.Domain.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.Property(w => w.ConcurrencyToken).IsRowVersion();
        builder.Property(w => w.Balance).HasPrecision(Wallet.BalancePrecision, Wallet.BalanceScale).IsRequired();
        
        builder.HasIndex(w => w.UserProfileId).IsUnique();
    }
}
namespace MovieStore.Domain.Users;

public class Wallet
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    
    public decimal Balance { get; set; }
    public const int BalancePrecision = 18;
    public const int BalanceScale = 2;
    
    public byte[] ConcurrencyToken { get; set; }
    
    public UserProfile UserProfile { get; set; }
}
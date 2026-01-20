namespace MovieStore.Domain.Users;

public class UserProfile(int identityUserId, string? name, Sex? sex)
{
    public int Id { get; set; }
    public int IdentityUserId { get; set; } = identityUserId;
    
    public string? Name { get; set; } = name;
    public const int NameMaxLength = 150;
    
    public Sex? Sex { get; set; } = sex;
    public DateTime CreatedAt { get; init; }
    
    public Wallet? Wallet { get; set; }
}
namespace MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

public class RefreshToken
{
    public long Id { get; set; }
    public Guid Value { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public int IdentityUserId { get; set; }
    
    public IdentityUserEntity IdentityUser { get; set; }
}
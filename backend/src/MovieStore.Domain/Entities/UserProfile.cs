using MovieStore.Domain.Enums;

namespace MovieStore.Domain.Entities;

public class UserProfile
{
    public int Id { get; set; }
    public int IdentityUserId { get; set; }
    public string? Name { get; set; }
    public Sex? Sex  { get; set; }
}
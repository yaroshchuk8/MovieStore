namespace MovieStore.Domain.Users;

public class PublisherProfile
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    
    public string StudioName { get; set; }
    public int StudioNameMaxLength = 150;
    
    public DateTime CreatedAt { get; init; }
    
    public UserProfile? UserProfile { get; set; }
}
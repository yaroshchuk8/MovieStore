using MovieStore.Domain.Movies;

namespace MovieStore.Domain.Users;

public class PublisherProfile
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    
    public string StudioName { get; set; }
    public const int StudioNameMaxLength = 150;
    
    public DateTime CreatedAt { get; init; }
    
    public UserProfile UserProfile { get; set; }
    public List<Movie> Movies { get; set; }
}
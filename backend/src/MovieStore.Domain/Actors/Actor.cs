using MovieStore.Domain.Movies;

namespace MovieStore.Domain.Actors;

public class Actor(string name, string? imageKey)
{
    public int Id { get; set; }
    
    public string Name { get; set; } = name;
    public const int NameMaxLength = 150;
    
    public string? ImageKey { get; set; } = imageKey;
    public const int ImageKeyMaxLength = 100;
    
    public DateTime CreatedAt { get; init; }

    public List<MovieActor> MovieActors { get; set; }
}
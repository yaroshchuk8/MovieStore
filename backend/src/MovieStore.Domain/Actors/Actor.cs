using MovieStore.Domain.Movies;

namespace MovieStore.Domain.Actors;

public class Actor(string name, string? imagePath)
{
    public int Id { get; set; }
    
    public string Name { get; set; } = name;
    public const int NameMaxLength = 150;
    
    public string? ImagePath { get; set; } = imagePath;
    public const int ImagePathMaxLength = 100;
    
    public DateTime CreatedAt { get; init; }

    public List<MovieActor> MovieActors { get; set; }
}
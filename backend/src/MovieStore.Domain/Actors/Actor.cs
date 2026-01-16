using MovieStore.Domain.Movies;

namespace MovieStore.Domain.Actors;

public class Actor(string name, string? imagePath)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string? ImagePath { get; set; } = imagePath;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<MovieActor> MovieActors { get; set; }
}
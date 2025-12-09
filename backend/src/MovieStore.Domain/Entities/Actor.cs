namespace MovieStore.Domain.Entities;

public class Actor(string name, string? imagePath)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string? ImagePath { get; set; } = imagePath;
    public DateTime CreateAt { get; set; } = DateTime.Now;

    public List<MovieActor> MovieActors { get; set; }
}
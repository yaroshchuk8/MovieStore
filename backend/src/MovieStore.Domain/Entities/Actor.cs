namespace MovieStore.Domain.Entities;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;

    public List<MovieActor> MovieActors { get; set; }
}
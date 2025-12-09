namespace MovieStore.Domain.Entities;

public class Movie(string title, string description, double price)
{
    public int Id { get; set; }
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public double Price { get; set; } = price;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public List<MovieGenre> MovieGenres { get; set; }
    public List<MovieActor> MovieActors { get; set; }
}
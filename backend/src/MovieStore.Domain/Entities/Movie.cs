namespace MovieStore.Domain.Entities;

public class Movie
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImagePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public List<MovieGenre> MovieGenres { get; set; }
    public List<MovieActor> MovieActors { get; set; }
}
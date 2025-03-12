namespace MovieStore.Domain.Entities;

public class Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImagePath { get; set; }

    // Navigation properties for many-to-many relationships
    public List<Genre> Genres { get; set; } = new();
    public List<Actor> Actors { get; set; } = new();
}
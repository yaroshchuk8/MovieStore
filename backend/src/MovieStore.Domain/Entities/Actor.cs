namespace MovieStore.Domain.Entities;

public class Actor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }

    // Navigation property for many-to-many relationship
    public List<Movie> Movies { get; set; } = new();
}
namespace MovieStore.Domain.Entities;

public class Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Navigation property for many-to-many relationship
    public List<Movie> Movies { get; set; } = new();
}
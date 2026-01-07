using MovieStore.Domain.Movies;

namespace MovieStore.Domain.Genres;

public class Genre(string name, string? description)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<MovieGenre> MovieGenres { get; set; }
}
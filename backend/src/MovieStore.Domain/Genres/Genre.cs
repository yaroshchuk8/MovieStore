using MovieStore.Domain.Movies;

namespace MovieStore.Domain.Genres;

public class Genre(string name, string? description)
{
    public int Id { get; set; }
    
    public string Name { get; set; } = name;
    public const int NameMaxLength = 50;
    
    public string? Description { get; set; } = description;
    public const int DescriptionMaxLength = 1000;
    
    public DateTime CreatedAt { get; init; }

    public List<MovieGenre> MovieGenres { get; set; }
}
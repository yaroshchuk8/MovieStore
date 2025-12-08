namespace MovieStore.Domain.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<MovieGenre> MovieGenres { get; set; }

    public Genre(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
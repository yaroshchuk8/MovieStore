namespace MovieStore.Domain.Entities;

public class Genre
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public List<MovieGenre> MovieGenres { get; set; }
}
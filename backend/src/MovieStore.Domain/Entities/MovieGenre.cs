namespace MovieStore.Domain.Entities;

public class MovieGenre
{
    public long MovieId { get; set; }
    public long GenreId { get; set; }
    
    public Movie Movie { get; set; }
    public Genre Genre { get; set; }
}
using MovieStore.Api.Genres;

namespace MovieStore.Api.Movies.Entities;

public class MovieGenre(int movieId, int genreId)
{
    public int MovieId { get; set; } = movieId;
    public int GenreId { get; set; }  = genreId;
    
    public Movie Movie { get; set; }
    public Genre Genre { get; set; }
}
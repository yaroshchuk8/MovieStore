using MovieStore.Application.Movies.DTOs;

namespace MovieStore.Application.Genres.DTOs;

public class GenreDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<MovieSummaryDto> Movies { get; set; }
}
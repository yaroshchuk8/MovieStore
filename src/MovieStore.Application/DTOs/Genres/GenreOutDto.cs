using MovieStore.Application.DTOs.Movies;

namespace MovieStore.Application.DTOs.Genres;

public class GenreOutDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<MovieSmallOutDto> Movies { get; set; }
}
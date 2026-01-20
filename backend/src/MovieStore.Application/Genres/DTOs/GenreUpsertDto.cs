namespace MovieStore.Application.Genres.DTOs;

public class GenreUpsertDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<long> MovieIds { get; set; }
}
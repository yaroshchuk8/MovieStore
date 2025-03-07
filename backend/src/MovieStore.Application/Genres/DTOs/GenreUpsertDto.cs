namespace MovieStore.Application.Genres.DTOs;

public class GenreUpsertDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid> MovieIds { get; set; }
}
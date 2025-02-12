namespace MovieStore.Application.DTOs.Genres;

public class GenreInDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid> MovieIds { get; set; }
}
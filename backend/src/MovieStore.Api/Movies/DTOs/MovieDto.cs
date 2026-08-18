namespace MovieStore.Application.Movies.DTOs;

public class MovieDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
}
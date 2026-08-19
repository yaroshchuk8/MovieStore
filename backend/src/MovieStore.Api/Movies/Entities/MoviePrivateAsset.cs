namespace MovieStore.Api.Movies.Entities;

public class MoviePrivateAsset
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string Key { get; set; }
    
    public Movie Movie { get; set; }
}
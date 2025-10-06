namespace MovieStore.Domain.Entities;

public class MovieActor
{
    public long MovieId { get; set; }
    public long ActorId { get; set; }
    public string? CharacterName { get; set; }
    
    public Movie Movie { get; set; }
    public Actor Actor { get; set; }
}
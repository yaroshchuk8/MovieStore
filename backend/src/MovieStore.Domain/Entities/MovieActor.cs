namespace MovieStore.Domain.Entities;

public class MovieActor(int movieId, int actorId, string? characterName)
{
    public int MovieId { get; set; } = movieId;
    public int ActorId { get; set; } = actorId;
    public string? CharacterName { get; set; } = characterName;
    
    public Movie Movie { get; set; }
    public Actor Actor { get; set; }
}
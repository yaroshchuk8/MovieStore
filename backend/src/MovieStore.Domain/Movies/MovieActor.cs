using MovieStore.Domain.Actors;

namespace MovieStore.Domain.Movies;

public class MovieActor(int movieId, int actorId, string? characterName = null)
{
    public int MovieId { get; set; } = movieId;
    public int ActorId { get; set; } = actorId;
    
    public string? CharacterName { get; set; } = characterName;
    public const int  CharacterNameMaxLength = 150;
    
    public Movie Movie { get; set; }
    public Actor Actor { get; set; }
}
namespace MovieStore.Api.Actors.Contracts;

// No constructor because of [FromForm] binding specifics
public record CreateActorRequest
{
    public required string Name { get; init; }
    public IFormFile? Image { get; init; }
}
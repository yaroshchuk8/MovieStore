namespace MovieStore.Contracts.Genres.Requests;

public record CreateGenreRequest(string Name, string? Description);
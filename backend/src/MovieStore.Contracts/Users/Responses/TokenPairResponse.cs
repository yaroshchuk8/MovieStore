namespace MovieStore.Contracts.Users.Responses;

public record TokenPairResponse(string Jwt, Guid RefreshToken);
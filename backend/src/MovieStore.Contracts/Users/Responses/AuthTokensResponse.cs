namespace MovieStore.Contracts.Users.Responses;

public record AuthTokensResponse(string AccessToken, Guid RefreshToken);
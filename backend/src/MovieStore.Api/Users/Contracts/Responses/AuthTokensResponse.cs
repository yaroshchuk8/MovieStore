namespace MovieStore.Api.Users.Contracts.Responses;

public record AuthTokensResponse(string AccessToken, Guid RefreshToken);
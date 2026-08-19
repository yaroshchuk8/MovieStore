namespace MovieStore.Api.Users.DTOs;

public record AuthTokens(string AccessToken, Guid RefreshToken);
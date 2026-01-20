namespace MovieStore.Application.Users.DTOs;

public record AuthTokens(string AccessToken, Guid RefreshToken);
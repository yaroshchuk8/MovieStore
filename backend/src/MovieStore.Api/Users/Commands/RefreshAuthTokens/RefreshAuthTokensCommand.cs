namespace MovieStore.Application.Users.Commands.RefreshAuthTokens;

public record RefreshAuthTokensCommand(string AccessToken, Guid RefreshToken);
namespace MovieStore.Api.Users.Commands.RefreshAuthTokens;

public record RefreshAuthTokensCommand(string AccessToken, Guid RefreshToken);
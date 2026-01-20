namespace MovieStore.Api.Contracts.Users.Requests;

public record RefreshAuthTokensRequest(string AccessToken, Guid RefreshToken);
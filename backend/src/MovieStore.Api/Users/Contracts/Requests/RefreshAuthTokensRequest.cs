namespace MovieStore.Api.Users.Contracts.Requests;

public record RefreshAuthTokensRequest(string AccessToken, Guid RefreshToken);
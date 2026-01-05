namespace MovieStore.Contracts.Users.Requests;

public record RefreshAuthTokensRequest(string AccessToken, Guid RefreshToken);
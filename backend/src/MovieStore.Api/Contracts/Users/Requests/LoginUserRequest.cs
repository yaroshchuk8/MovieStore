namespace MovieStore.Api.Contracts.Users.Requests;

public record LoginUserRequest(string Email, string Password);
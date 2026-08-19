namespace MovieStore.Api.Users.Contracts.Requests;

public record LoginUserRequest(string Email, string Password);
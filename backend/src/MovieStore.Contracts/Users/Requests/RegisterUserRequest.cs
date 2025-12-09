namespace MovieStore.Contracts.Users.Requests;

public record RegisterUserRequest(string Email, string Password, string? Name, int? Sex);
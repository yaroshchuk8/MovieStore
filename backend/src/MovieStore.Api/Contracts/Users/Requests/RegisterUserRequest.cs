using MovieStore.Domain.Users;

namespace MovieStore.Api.Contracts.Users.Requests;

public record RegisterUserRequest(string Email, string Password, string? Name, Sex? Sex);
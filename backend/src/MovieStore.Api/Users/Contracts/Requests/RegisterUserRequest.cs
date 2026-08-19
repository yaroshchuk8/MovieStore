using MovieStore.Api.Users.Contracts.Common;

namespace MovieStore.Api.Users.Contracts.Requests;

public record RegisterUserRequest(string Email, string Password, string? Name, Sex? Sex);
using MovieStore.Api.Users.Entities.Domain.Enums;

namespace MovieStore.Api.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string? Name, Sex? Sex);
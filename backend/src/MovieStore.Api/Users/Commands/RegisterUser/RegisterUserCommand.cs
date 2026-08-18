using MovieStore.Domain.Users.Enums;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string? Name, Sex? Sex);
using MediatR;
using MovieStore.Contracts.Users.Responses;
using ErrorOr;

namespace MovieStore.Application.Users.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<ErrorOr<AuthTokensResponse>>;
using ErrorOr;
using MediatR;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Users;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterUserCommand, ErrorOr<(string AccessToken, Guid RefreshToken)>>
{
    public async Task<ErrorOr<(string AccessToken, Guid RefreshToken)>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        const Role role = Role.Customer;
        var registrationResult = await identityService.CreateUserAndGenerateAuthTokensAsync(
            email: request.Email,
            password: request.Password,
            name: request.Name,
            sex: request.Sex,
            role: role);
        if (registrationResult.IsError)
        {
            return registrationResult.Errors;
        }

        return (registrationResult.Value.AccessToken, registrationResult.Value.RefreshToken);
    }
}
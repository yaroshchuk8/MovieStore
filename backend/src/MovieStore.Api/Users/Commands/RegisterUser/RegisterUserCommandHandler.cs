using ErrorOr;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Api.Users.DTOs;
using MovieStore.Api.Users.Entities.Domain.Enums;
using MovieStore.Api.Users.Services.Interfaces;

namespace MovieStore.Api.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterUserCommand, AuthTokens>
{
    public async Task<ErrorOr<AuthTokens>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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

        return registrationResult.Value.AuthTokens;
    }
}
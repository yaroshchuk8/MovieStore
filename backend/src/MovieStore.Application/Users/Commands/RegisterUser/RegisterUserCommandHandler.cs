using ErrorOr;
using MediatR;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Contracts.Users.Responses;
using MovieStore.Domain.Enums;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterUserCommand, ErrorOr<AuthTokensResponse>>
{
    public async Task<ErrorOr<AuthTokensResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        const Role role = Role.Customer;
        var registerUserResult = await identityService.CreateUserAndGenerateTokenPairAsync(
            email: request.Email,
            password: request.Password,
            name: request.Name,
            sex: request.Sex,
            role: role);
        if (registerUserResult.IsError)
        {
            return registerUserResult.Errors;
        }
        var (_, tokenPair) = registerUserResult.Value;
        
        return tokenPair;
    }
}
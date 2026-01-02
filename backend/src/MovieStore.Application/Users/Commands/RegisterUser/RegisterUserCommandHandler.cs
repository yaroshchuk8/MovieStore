using ErrorOr;
using MediatR;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Contracts.Users.Responses;
using MovieStore.Domain.Enums;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IIdentityService identityService, IJwtService jwtService)
    : IRequestHandler<RegisterUserCommand, ErrorOr<TokenPairResponse>>
{
    public async Task<ErrorOr<TokenPairResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var role = Role.Customer;
        var registerUserResult = await identityService.CreateUserAndGenerateRefreshTokenAsync(
            email: request.Email,
            password: request.Password,
            name: request.Name,
            sex: request.Sex,
            role: Role.Customer);
        if (registerUserResult.IsError)
        {
            return registerUserResult.Errors;
        }
        var (identityUserContract, refreshToken) = registerUserResult.Value;
        
        var roleName = role.ToString();
        var jwtToken = jwtService.GenerateJwt(identityUserContract, [roleName]);
        
        return new TokenPairResponse(jwtToken, refreshToken);
    }
}
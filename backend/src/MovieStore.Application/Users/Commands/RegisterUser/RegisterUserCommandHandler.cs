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
        var registerUserResult = await identityService.RegisterUserAsync(
            email: request.Email,
            password: request.Password,
            name: request.Name,
            sex: request.Sex,
            role: Role.Customer);
        if (registerUserResult.IsError)
        {
            return registerUserResult.Errors;
        }
        
        var identityUserContract =  registerUserResult.Value;
        var roleName = role.ToString();
        var jwtToken = jwtService.GenerateJwt(identityUserContract, [roleName]);
        
        var generateRefreshTokenResult = await identityService.GenerateRefreshTokenAsync(identityUserContract.Id);
        if (generateRefreshTokenResult.IsError)
        {
            return generateRefreshTokenResult.Errors;
        }
        var refreshToken = generateRefreshTokenResult.Value;
        
        return new TokenPairResponse(jwtToken, refreshToken);
    }
}
using ErrorOr;
using MediatR;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUserManagementService userManagementService, IJwtService jwtService)
    : IRequestHandler<RegisterUserCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var identityUserResult = await userManagementService.RegisterUserAsync(
            email: request.Email,
            password: request.Password,
            name: request.Name,
            sex: request.Sex);
        if (identityUserResult.IsError)
        {
            return identityUserResult.Errors;
        }
        
        var jwtToken = await jwtService.GenerateJwtToken(identityUserResult.Value);
        
        return jwtToken;
    }
}
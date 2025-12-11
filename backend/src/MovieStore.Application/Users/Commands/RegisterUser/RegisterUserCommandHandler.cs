using ErrorOr;
using MediatR;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUserManagementService userManagementService)
    : IRequestHandler<RegisterUserCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await userManagementService.RegisterUserAsync(
            email: request.Email,
            password: request.Password,
            name: request.Name,
            sex: request.Sex);

        return result;
    }
}
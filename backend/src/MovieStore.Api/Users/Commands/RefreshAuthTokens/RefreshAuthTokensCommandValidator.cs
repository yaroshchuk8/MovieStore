using FluentValidation;

namespace MovieStore.Application.Users.Commands.RefreshAuthTokens;

public class RefreshAuthTokensCommandValidator : AbstractValidator<RefreshAuthTokensCommand>
{
    public RefreshAuthTokensCommandValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Access token is required.")
            .MaximumLength(8192).WithMessage("Access token is too long.")
            .Matches(@"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+$").WithMessage("Invalid access token format.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .NotEqual(Guid.Empty).WithMessage("Refresh token cannot be an empty GUID.");
    }
}
namespace MovieStore.Application.Users.Interfaces;

public interface ICurrentUserProvider
{
    int? IdentityUserId { get; }
    string? Email { get; }
    IReadOnlyList<string> Roles { get; }
}
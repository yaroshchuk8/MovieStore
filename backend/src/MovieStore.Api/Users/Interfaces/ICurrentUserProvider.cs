namespace MovieStore.Application.Users.Interfaces;

public interface ICurrentUserProvider
{
    int? IdentityUserId { get; }
    string? IdentityUserName { get; }
    string? IdentityUserEmail { get; }
    IReadOnlyList<string> Roles { get; }
    int? DomainUserId { get; }
}
namespace MovieStore.Application.Users.Interfaces;

public interface IIdentityUserContract
{
    int Id { get; set; }
    string? UserName { get; set; }
    string? Email { get; set; }
}
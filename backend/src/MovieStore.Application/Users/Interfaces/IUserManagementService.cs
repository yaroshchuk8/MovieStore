using ErrorOr;
using MovieStore.Domain.Enums;

namespace MovieStore.Application.Users.Interfaces;

public interface IUserManagementService
{
    Task<ErrorOr<Success>> RegisterUserAsync(string email, string password, string? name, Sex? sex);
}
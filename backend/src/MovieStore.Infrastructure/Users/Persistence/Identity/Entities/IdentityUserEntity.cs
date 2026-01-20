using Microsoft.AspNetCore.Identity;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

public class IdentityUserEntity : IdentityUser<int>, IIdentityUserContract
{
    
}
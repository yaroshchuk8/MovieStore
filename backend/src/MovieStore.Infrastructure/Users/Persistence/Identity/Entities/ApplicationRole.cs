using Microsoft.AspNetCore.Identity;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() : base() { }
    
    public ApplicationRole(string roleName) : base(roleName) { }
}
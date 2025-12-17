using Microsoft.AspNetCore.Identity;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

public class IdentityRoleEntity : IdentityRole<int>
{
    public IdentityRoleEntity() : base() { }
    
    public IdentityRoleEntity(string roleName) : base(roleName) { }
}
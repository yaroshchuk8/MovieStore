using Microsoft.AspNetCore.Identity;

namespace MovieStore.Api.Users.Entities.Identity;

public class IdentityRoleEntity : IdentityRole<int>
{
    public IdentityRoleEntity() : base() { }
    
    public IdentityRoleEntity(string roleName) : base(roleName) { }
}
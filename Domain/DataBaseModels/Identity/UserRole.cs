using Domain.Core.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.DataBaseModels.Identity
{
    public class UserRole: IdentityRole, IBaseEntity<string>
    {
        public UserRole(string roleName) : base(roleName) { }
        public UserRole() { }
    }
}

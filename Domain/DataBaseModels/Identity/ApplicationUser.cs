using Domain.Core.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.DataBaseModels.Identity
{
    public class ApplicationUser : IdentityUser, IBaseEntity<string>
    {

    }
}

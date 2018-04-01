using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Core;
using Domain.DataBaseModels.Identity;

namespace Servises.Interfaces.AuthenticationServices
{
    /// <summary>
    /// Provides the APIs for user sign in.
    /// </summary>
    public interface ISignInService
    {
        /// <summary>
        /// Signs in the specified user.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null);

        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SignOutAsync();

        /// <summary>
        /// Creates a System.Security.Claims.ClaimsPrincipal for the specified user, as an
        /// asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create a System.Security.Claims.ClaimsPrincipal for.</param>
        /// <returns>The task object representing the asynchronous operation, containing the ClaimsPrincipal
        /// for the specified user.</returns>
        Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user);

        /// <summary>
        /// Attempts to sign in the specified userName and password combination as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="userName">The user name to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the SignInResult for the sign-in attempt.</returns>
        Task<ServiceResult> PasswordSignInAsync(string userName, string password, bool isPersistent,
            bool lockoutOnFailure = true);

        /// <summary>
        /// Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if 
        /// the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing 
        /// the ServiceResult for the sign-in attempt.</returns>
        Task<ServiceResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure);
    }
}

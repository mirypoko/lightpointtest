using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Core;
using Domain.DataBaseModels.Identity;
using Microsoft.AspNetCore.Identity;
using Servises.Interfaces.Base;

namespace Servises.Interfaces.AuthenticationServices
{
    /// <summary>
    /// Provides the APIs for managing user in a persistence store.
    /// </summary>
    public interface IUserService: IBaseGenericDataService<string, ApplicationUser>
    {
        /// <summary>
        /// Adds an external Microsoft.AspNetCore.Identity.UserLoginInfo to the specified
        /// user.
        /// </summary>
        /// <param name="user">The user to add the login to.</param>
        /// <param name="login">The external Microsoft.AspNetCore.Identity.UserLoginInfo to add to the specified user.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> AddLoginAsync(ApplicationUser user, Domain.DataBaseModels.Identity.UserLoginInfo login);

        /// <summary>
        /// Adds an external user login info to the specified user.
        /// </summary>
        /// <param name="user">The user to add the login to.</param>
        /// <param name="loginProvider">The provider. Examples of the provider may be Local, Facebook, Google, etc.</param>
        /// <param name="providerKey">Unique identifier for the user provided by the login provider.</param>
        /// <param name="displayName">This would be unique per provider, examples may be @microsoft as a Twitter provider key.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> AddLoginAsync(ApplicationUser user, string loginProvider, string providerKey,
            string displayName);

        /// <summary>
        /// Adds the specified claims to the user.
        /// </summary>
        /// <param name="user">The user to add the claim to.</param>
        /// <param name="claims">The claims to add.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims);

        /// <summary>
        /// Finds and returns a user, if any, who has the specified user name.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        ///  the user matching the specified userName if it exists.</returns>
        Task<ApplicationUser> FindByNameAsync(string userName);

        /// <summary>
        /// Retrieves the user associated with the specified external login provider and
        /// login provider key.
        /// </summary>
        /// <param name="loginProvider">The login provider who provided the providerKey.</param>
        /// <param name="providerKey">The key provided by the loginProvider to identify a user.</param>
        /// <returns>The System.Threading.Tasks.Task for the asynchronous operation, containing the
        /// user, if any which matched the specified login provider and key.</returns>
        Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey);

        /// <summary>
        /// Gets the user, if any, associated with the normalized value of the specified
        /// email address.
        /// </summary>
        /// <param name="email">The email address to return the user for.</param>
        /// <returns>The task object containing the results of the asynchronous lookup operation,
        /// the user, if any, associated with a normalized value of the specified email address.</returns>
        Task<ApplicationUser> FindByEmailAsync(string email);

        /// <summary>
        /// Get user by user name or email.
        /// </summary>
        /// <param name="userNameOrEmail">User name or email.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ApplicationUser of the operation or null.</returns>
        Task<ApplicationUser> GetByUserNameOrEmailOrDefaultAsync(string userNameOrEmail);

        /// <summary>
        /// Get user by user name.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ApplicationUser of the operation or null.</returns>
        Task<ApplicationUser> GetByUserNameOrDefaultAsync(string userName);

        /// <summary>
        /// Get user by email.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ApplicationUser of the operation or null.</returns>
        Task<ApplicationUser> GetByEmailOrDefaultAsync(string email);

        /// <summary>
        /// Lock an user.
        /// </summary>
        /// <param name="user">The user to lock.</param>
        /// <param name="date">Date then unlock.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> LockOutAsync(ApplicationUser user, DateTime date);

        /// <summary>
        /// Gets a list of role names the specified user belongs to.
        /// </summary>
        /// <param name="user">The user whose role names to retrieve.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing a list of role names.</returns>
        Task<List<string>> GetRolesAsync(ApplicationUser user);

        /// <summary>
        /// Removes the specified user from the named role.
        /// </summary>
        /// <param name="user">The user to remove from the named role</param>
        /// <param name="role">The name of the role to remove the user from.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> RemoveFromRoleAsync(ApplicationUser user, string role);

        /// <summary>
        /// Add the specified user to the named role.
        /// </summary>
        /// <param name="user">The user to add to the named role.</param>
        /// <param name="role">The name of the role to add the user to.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> AddToRoleAsync(ApplicationUser user, string role);

        /// <summary>
        /// Returns a flag indicating whether the specified user is a member of the give
        /// named role.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="role">The name of the role to be checked.</param>
        /// <returns></returns>
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);

        /// <summary>
        /// Changes a user's password after confirming the specified currentPassword is correct, 
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose password should be set.</param>
        /// <param name="currentPassword">The current password to validate before changing.</param>
        /// <param name="newPassword">The new password to set for the specified user.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

        /// <summary>
        /// Generates an email confirmation token for the specified user.
        /// </summary>
        /// <param name="user">The user to generate an email confirmation token for.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// an email confirmation token.</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);

        /// <summary>
        /// Validates that an email confirmation token matches the specified user.
        /// </summary>
        /// <param name="user">The user to validate the token against.</param>
        /// <param name="code">The email confirmation token to validate.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> ConfirmEmailAsync(ApplicationUser user, string code);

        /// <summary>
        /// Generates a password reset token for the specified user, using the configured 
        /// password reset token provider.
        /// </summary>
        /// <param name="user">The user to generate a password reset token for.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing a password reset token for the specified user.</returns>
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

        /// <summary>
        /// Resets the user's password to the specified newPassword after validating 
        /// the given password reset token.
        /// </summary>
        /// <param name="user">The user whose password should be reset.</param>
        /// <param name="token">The password reset token to verify.</param>
        /// <param name="newPassword">The new password to set if reset token verification fails.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);

        /// <summary>
        /// Creates the specified user in the backing store with given password, 
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The password for the user to hash and store.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> CreateAsync(ApplicationUser user, string password);

        /// <summary>
        /// Creates the specified user in the backing store with no password, as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> CreateAsync(ApplicationUser user);

        /// <summary>
        /// Updates the specified user in the backing store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> UpdateAsync(ApplicationUser user);

        /// <summary>
        /// Deletes the specified user from the backing store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> DeleteAsync(ApplicationUser user);

        /// <summary>
        /// Returns a flag indicating whether the given password is valid for the specified user.
        /// </summary>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The System.Threading.Tasks.Task that represents the asynchronous 
        /// operation, containing true if the specified password matches the one store for the user, 
        /// otherwise false.</param>
        /// <returns></returns>
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// Returns a current user.
        /// </summary>
        /// <returns>A current user or null.</returns>
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}

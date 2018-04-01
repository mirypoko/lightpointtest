using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Core;
using Domain.DataBaseModels.Identity;
using Microsoft.AspNetCore.Identity;
using Servises.Interfaces.AuthenticationServices;

namespace AspNetIdentityAuthenticationServices
{
    public class SignInService : ISignInService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null)
        {
            return _signInManager.SignInAsync(user, isPersistent, authenticationMethod);
        }

        public Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user)
        {
            return _signInManager.CreateUserPrincipalAsync(user);
        }

        public Task SignOutAsync()
        {
            return _signInManager.SignOutAsync();
        }

        public async Task<ServiceResult> PasswordSignInAsync(string userName, string password, bool rememberLogin,bool lockoutOnFailure = true)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, rememberLogin, lockoutOnFailure);
            return SingInResultToServiceResult(result);
        }

        public async Task<ServiceResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return SingInResultToServiceResult(result);
        }

        protected ServiceResult SingInResultToServiceResult(SignInResult signInResult)
        {
            if (signInResult.Succeeded) return new ServiceResult(true);

            var errors = new List<string>();

            if (signInResult.IsLockedOut) errors.Add("User is lock");

            if (signInResult.IsNotAllowed) errors.Add("User is not allowwed");

            errors.Add("Login failed");

            return new ServiceResult(false, errors);
        }
    }
}

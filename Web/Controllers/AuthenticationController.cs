using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Identity;
using DumbledoreMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servises.Interfaces.AuthenticationServices;
using Web.Extensions;
using Web.Initializers;
using Web.ViewModels.Authorization;
using Web.ViewModels.User;

namespace Web.Controllers
{
    /// <inheritdoc />
    [Authorize]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly ISignInService _signInService;

        private readonly IUserService _userService;

        private readonly IJwtTokensService _jwtTokenServices;

        /// <inheritdoc />
        public AuthenticationController(
            IJwtTokensService jwtTokenServices, ISignInService signInService, IUserService userService)
        {
            _jwtTokenServices = jwtTokenServices;
            _signInService = signInService;
            _userService = userService;
        }

        /// <summary>
        /// User is authenticated.
        /// </summary>
        /// <response code="200">Success.</response>
        [HttpGet("IsAuthenticated")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Boolean), 200)]
        public bool IsAuthenticatedAsync()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Get current user.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="404">User is not found.</response>
        [HttpGet("current")]
        [ProducesResponseType(typeof(UserGetViewModel), 200)]
        public async Task<IActionResult> CurrentAsync()
        {
            var user = await _userService.GetCurrentUserAsync();

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = Mapper.Map<UserGetViewModel>(user);
            var roles = await _userService.GetRolesAsync(user);
            viewModel.Roles = new List<string>();
            foreach (var role in roles)
            {
                viewModel.Roles.Add(role);
            }
            return Ok(viewModel);
        }

        /// <summary>
        /// Login with cookies.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="remember"></param>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid password.</response>
        /// <response code="404">User with received email or name not found.</response>
        [AllowAnonymous]
        [HttpPost("Cookies")]
        [ProducesResponseType(typeof(JwtToken), 200)]
        public async Task<IActionResult> Cookies([FromBody]SingInViewModel model, bool remember = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorsToList());
            }

            var normalizedUserName = model.Username.Trim().ToUpper();

            var user = await _userService.GetByUserNameOrEmailOrDefaultAsync(normalizedUserName);

            if (user == null)
            {
                return NotFound($"User with email or name {model.Username} not found");
            }

            var result = await _signInService.PasswordSignInAsync(user.UserName, model.Password, remember, true);

            if (!result.Succeeded) return result.ToActionResult();

            return Ok();
        }

        /// <summary>
        /// Logout.
        /// </summary>
        /// <response code="200">Success.</response>
        [AllowAnonymous]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(JwtToken), 200)]
        public async Task<IActionResult> CookiesLogout()
        {
            await _signInService.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Get jwt token.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid password.</response>
        /// <response code="404">User with received email or name not found.</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(JwtToken), 200)]
        [HttpPost("GetJwtToken")]
        public async Task<IActionResult> GetJwtToken([FromBody]SingInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorsToList());
            }

            var normalizedUserName = model.Username.Trim().ToUpper();

            var user = await _userService.GetByUserNameOrEmailOrDefaultAsync(normalizedUserName);

            if (user == null)
            {
                return NotFound($"User with email or name {model.Username} not found");
            }

            var result = await _signInService.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return result.ToActionResult();

            return Ok(await _jwtTokenServices.GetJwtTokenAsync(user));
        }

        /// <summary>
        /// Get new jwt access token by refresh token.
        /// </summary>
        /// <param name="model">Refresh jwt token.</param>
        /// <response code="200">New access token.</response>
        /// <response code="400">Failed to get new access jwt token.</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(JwtToken), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        [HttpPost("UpdateJwtToken")]
        public async Task<IActionResult> UpdateJwtToken([FromBody]RefreshTokenViewModel model)
        {
            var token = await _jwtTokenServices.GetJwtTokenByRefreshTokenAsync(model.RefreshToken);
            if (token == null)
            {
                return BadRequest();
            }

            return Ok(token);
        }
    }
}

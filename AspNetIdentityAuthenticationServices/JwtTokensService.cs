using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database.EntityFrameworkCore;
using Domain.DataBaseModels.Identity;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Servises.Interfaces.AuthenticationServices;
using Utils;
using Utils.NonStatic;

namespace AspNetIdentityAuthenticationServices
{
    public sealed class JwtTokensService : IJwtTokensService
    {
        private readonly IUserService _userService;

        private readonly IHttpUtilsService _httpUtilsService;

        private readonly ApplicationDbContext _dbContext;

        public JwtTokensService(IUserService userService, IHttpUtilsService httpUtilsService, ApplicationDbContext dbContext)
        {
            _userService = userService;
            _httpUtilsService = httpUtilsService;
            _dbContext = dbContext;
        }

        public async Task<JwtToken> GetJwtTokenByRefreshTokenAsync(string refreshToken)
        {
            var jwtRefreshToken = await _dbContext.JwtRefreshTokens.Include(t => t.ApplicationUser).FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
            if (jwtRefreshToken != null)
            {
                return new JwtToken()
                {
                    AccessToken = await GetAccessTokenAsync(jwtRefreshToken.ApplicationUser),
                    RefreshToken = refreshToken,
                    UserId = jwtRefreshToken.ApplicationUserId
                };
            }

            return null;
        }

        public async Task<JwtToken> GetJwtTokenAsync(ApplicationUser user)
        {

            var jwtAccessToken = await GetAccessTokenAsync(user);
            var jwtRefreshToken = GetRefreshToken(user);
            await AddRefreshTokenToDbAsync(jwtRefreshToken);

            return new JwtToken()
            {
                UserId = user.Id,
                AccessToken = jwtAccessToken,
                RefreshToken = jwtRefreshToken.RefreshToken
            };

        }

        public async Task DeleteUserRefreshTokensAsync(ApplicationUser user)
        {
            var tokens = await _dbContext.JwtRefreshTokens.Where(t => t.ApplicationUserId == user.Id).ToListAsync();
            _dbContext.JwtRefreshTokens.RemoveRange(tokens);
            await _dbContext.SaveChangesAsync();
        }

        private async Task AddRefreshTokenToDbAsync(JwtRefreshToken jwtRefreshToken)
        {
            await _dbContext.JwtRefreshTokens.AddAsync(jwtRefreshToken);
            await _dbContext.SaveChangesAsync();
        }

        private JwtRefreshToken GetRefreshToken(ApplicationUser user)
        {
            return new JwtRefreshToken()
            {
                ApplicationUserId = user.Id,
                ClientIp = _httpUtilsService.GetClientIpAddressString(),
                CreateDateTime = DateTime.Now,
                RefreshToken = user.Id + Randomizer.GetRandomString(300),
                UserAgent = _httpUtilsService.GetUserAgentString()
            };
        }

        private async Task<string> GetAccessTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userService.GetRolesAsync(user);

            var identity = GetClaimsIdentity(user, userRoles);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                AuthJwtOptions.Issuer,
                AuthJwtOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthJwtOptions.Lifetime)),

                signingCredentials: new SigningCredentials(AuthJwtOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static ClaimsIdentity GetClaimsIdentity(ApplicationUser user, IEnumerable<string> userRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultIssuer, AuthJwtOptions.Issuer),
                new Claim("UserName", user.UserName),
                new Claim("UserEmail", user.Email),
                new Claim("UserId", user.Id),
            };

            if (userRoles != null)
                claims.AddRange(userRoles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", "Role",
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}

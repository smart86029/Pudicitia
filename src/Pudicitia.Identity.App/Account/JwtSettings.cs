using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.App.Account
{
    public class JwtSettings
    {
        private readonly SymmetricSecurityKey securityKey;

        public JwtSettings(string key, string issuer, string audience, TimeSpan accessTokenExpiry, TimeSpan refreshTokenExpiry)
        {
            Key = key;
            Issuer = issuer;
            Audience = audience;
            AccessTokenExpiry = accessTokenExpiry;
            RefreshTokenExpiry = refreshTokenExpiry;
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }

        public string Key { get; }

        public string Issuer { get; }

        public string Audience { get; }

        public TimeSpan AccessTokenExpiry { get; }

        public TimeSpan RefreshTokenExpiry { get; }

        public string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                expires: DateTime.UtcNow.Add(AccessTokenExpiry),
                signingCredentials: credentials);
            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(securityToken);
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString("N");
        }

        public ClaimsPrincipal GetPrincipal(string accessToken)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = false
            };
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

            return result;
        }
    }
}
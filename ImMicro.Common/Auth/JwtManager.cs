using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ImMicro.Common.Auth.Concrete;
using ImMicro.Common.Constans;
using Microsoft.IdentityModel.Tokens;

namespace ImMicro.Common.Auth
{
     public class JwtManager
    {
        private static readonly byte[] SymmetricKey = Encoding.UTF8.GetBytes(AppConstants.SymmetricKey);

        public static TokenValidationParameters ValidationParameters => new TokenValidationParameters
        {
            RequireExpirationTime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(SymmetricKey)
        };

        public AccessTokenContract GenerateToken(JwtContract member)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(AppConstants.ClaimTypesId, member.Id.ToString()),
                    new Claim(ClaimTypes.Name, member.Name ?? string.Empty),
                    new Claim(ClaimTypes.Email, member.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, member.UserType?.ToString() ?? string.Empty),
                }),
                Expires = DateTime.UtcNow.AddMinutes(AppConstants.AccessTokenExpireMinute),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SymmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);

            return new AccessTokenContract
            {
                AccessToken = jwtToken.RawData,
                ExpiresIn = jwtToken.Payload.Exp,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpireDate = DateTime.UtcNow.AddMinutes(AppConstants.RefreshTokenExpireMinute)
            };
        }

        private static string GenerateRefreshToken()
        {
            var number = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Infrastructure.Infrastructure.Auth.JWT
{
    public interface IJwtHandler
    {
        LoginResponse Create(JwtRequest request);
    }

    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtSettings _options;
        private readonly SecurityKey _issuerSigningKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtHandler(IOptions<JwtSettings> options)
        {
            _options = options.Value;
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret));
            _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret))
            };
        }

        public LoginResponse Create(JwtRequest request)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(_options.ExpiryMinutes);
            var centuryBegins = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)new TimeSpan(expires.Ticks - centuryBegins.Ticks).TotalSeconds;
            var now = (long)new TimeSpan(nowUtc.Ticks - centuryBegins.Ticks).TotalSeconds;

            var payload = new JwtPayload
            {
                {"sub", request.UserId},
                {"iss", _options.Issuer},
                {"iat", now},
                {"exp", exp},
                {"unique_name", request.EmailAddress},
                {"PolicyCode", request.ProfileCode},
                {"AccountStatus", (int)request.AccountStatus},
                {"IsKycVerified", request.IsKycVerified}
            };

            if (request.UserType == UserType.Admin)
            {
                payload.Add("Email", request.EmailAddress);
                payload.Add("UserType", (int)request.UserType);
                payload.Add("Privilege", request.Privileges);
            }
            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new LoginResponse
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Email = request.EmailAddress,
                IsEmailVerified = request.IsEmailVerified,
                UserType = request.UserType,
                AccountStatus = request.AccountStatus,
                Token = token,
                Expires = (long)new TimeSpan(expires.Ticks).TotalSeconds,
                UserProfilePhoto = request.UserProfilePhoto,
                Privileges = request.Privileges,
                RoleText = request.RoleText,
                RoleId = request.RoleId

                //RoleId = request.RoleId
            };
        }
    }
}

using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public JwtHandler(IOptions<JwtSettings> options, IConfiguration configuration)
        {
            _options = options.Value;
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret));
            _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _configuration = configuration;
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]!))
            };
            
        }

        public LoginResponse Create(JwtRequest request)
        {
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]!);
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(525600);
            var claims = new List<Claim>
{
         new Claim("sub", request.UserId.ToString()),
         new Claim(JwtRegisteredClaimNames.Iat, nowUtc.ToString(), ClaimValueTypes.Integer64),
         new Claim("unique_name", request.EmailAddress),
         new Claim("PolicyCode", request.ProfileCode),
            new Claim("AccountStatus", ((int)request.AccountStatus).ToString()),
            new Claim(JwtRegisteredClaimNames.Iss,issuer!),
            new Claim(JwtRegisteredClaimNames.Aud,audience!),
                };
            if (request.UserType == UserType.Admin)
            {
                claims.Add(new Claim("Email", request.EmailAddress));
                claims.Add(new Claim("UserType", ((int)request.UserType).ToString()));
                claims.Add(new Claim("Privilege", request.Privileges));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);


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
                Token = jwtToken,
                Expires = (long)new TimeSpan(expires.Ticks).TotalSeconds,
                Privileges = request.Privileges,
                RoleText = request.RoleText,
                RoleId = request.RoleId
            };
        }
    }
}

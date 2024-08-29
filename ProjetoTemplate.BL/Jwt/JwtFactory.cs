using Microsoft.Extensions.Options;
using ProjetoTemplate.Domain.DTO.Authentication;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL.Jwt
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory()
        {
            _jwtOptions = new JwtIssuerOptions();
        }

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateSAMLEncodedToken(ClaimsPrincipal claims)
        {
            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: JwtIssuerOptions.Issuer,
                audience: JwtIssuerOptions.Audience,
                claims: claims.Claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
                     {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Rol),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Id),
                 new Claim("permission","read")
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: JwtIssuerOptions.Issuer,
                audience: JwtIssuerOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        /// <summary>
        /// Receives a custom claim DTO and generates a token
        /// </summary>
        /// <param name="userClaimsDTO"></param>
        /// <returns>token as string</returns>
        public string GenerateEncodedToken(UserDTO userClaimsDTO)
        {
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userClaimsDTO.Login ?? string.Empty),
                 new Claim(JwtRegisteredClaimNames.Email, userClaimsDTO.Email),
                 new Claim("userName", userClaimsDTO.Name),
                 new Claim("userId", userClaimsDTO.Id.ToString()),
                 new Claim("userProfile", userClaimsDTO.Profile),                 
                 new Claim("isProfileAdmin", userClaimsDTO.IsProfileAdmin.ToString()),
                 new Claim("isFirstAccess", userClaimsDTO.IsFirstAccess.ToString()),
                 new Claim("accessAllModules", userClaimsDTO.AccessAllModules.ToString()),
                 new Claim("moduleAccess", JsonSerializer.Serialize(userClaimsDTO.ModuleAccess)),

                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64)
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: JwtIssuerOptions.Issuer,
                audience: JwtIssuerOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "ApiUser"), new[]
            {
                new Claim(Constants.Strings.JwtClaimIdentifiers.Id, id),
                new Claim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess)
            });
        }

        private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() -
                             new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                            .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }

    public class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }
    }
}

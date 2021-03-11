using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Critic.Authentication
{
    public class GoogleTokenValidator : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private IConfiguration _configuration;

        public GoogleTokenValidator(IConfiguration configuration)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _configuration = configuration;
        }

        public bool CanValidateToken => _configuration.GetValue<bool?>("DisableTokenValidation") == null || _configuration.GetValue<bool?>("DisableTokenValidation") == false;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            var payload = GoogleJsonWebSignature.ValidateAsync(securityToken, new GoogleJsonWebSignature.ValidationSettings()).Result; // here is where I delegate to Google to validate

            var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, payload.Name),
                    new Claim(ClaimTypes.Name, payload.Name),
                    new Claim(JwtRegisteredClaimNames.FamilyName, payload.FamilyName),
                    new Claim(JwtRegisteredClaimNames.GivenName, payload.GivenName),
                    new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, payload.Subject),
                    new Claim(JwtRegisteredClaimNames.Iss, payload.Issuer),
                };

            try
            {
                var principal = new ClaimsPrincipal();
                principal.AddIdentity(new ClaimsIdentity(claims, "Password"));
                return principal;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;

            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        

        private static SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration){
            //THE KEY NEEDS ENOUGH CHARACTERS
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]));
        }

        public static string CreateSocialAuthToken(string email)
        {
            //Set the claims header, for now just the email
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, email)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            //Fill the token information
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "InfiniShopAPI",
                Issuer = "AuthService",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            //var to handle and create the token
            var tokenHandler = new JwtSecurityTokenHandler();
            //create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Return the token
            return tokenHandler.WriteToken(token);
        }

        public string CreateToken(AppUser user)
        {
            //Set the claims header, for now just the email
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.Email)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            //Fill the token information
            var tokenDescriptor = new SecurityTokenDescriptor{
                Audience = "InfiniShopAPI",
                Issuer = "AuthService",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            //var to handle and create the token
            var tokenHandler = new JwtSecurityTokenHandler();
            //create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Return the token
            return tokenHandler.WriteToken(token);
        }
    }
}
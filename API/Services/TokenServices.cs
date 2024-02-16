
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenServices : ITokenService
    {
        //To create a Jwt
        private readonly SymmetricSecurityKey _key;
        public TokenServices(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.NameId, user.UserName)   
            };
             
             var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

             //describe the token which to be returned
             var tokenDescriptor = new SecurityTokenDescriptor
             {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
             };

             var tokenHandler = new JwtSecurityTokenHandler();

             //create the token
             var token = tokenHandler.CreateToken(tokenDescriptor);

             return tokenHandler.WriteToken(token);

        }

       

    }
}
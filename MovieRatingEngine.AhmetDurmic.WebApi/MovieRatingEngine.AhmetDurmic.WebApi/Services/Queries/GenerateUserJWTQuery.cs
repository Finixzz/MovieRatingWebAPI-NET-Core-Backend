using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieRatingEngine.AhmetDurmic.WebApi.DTOS;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRatingEngine.AhmetDurmic.WebApi.Services.Queries
{
    public class GenerateUserJWTQuery : IRequest<object>
    {
        public AuthenticateUserDTO AuthenticateUserDTO { get; set; }

        public GenerateUserJWTQuery(AuthenticateUserDTO AuthenticateUserDTO)
        {
            this.AuthenticateUserDTO = AuthenticateUserDTO;
        }
    }

    public class GenerateUserJWTQueryHandler : IRequestHandler<GenerateUserJWTQuery, object>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;


        public GenerateUserJWTQueryHandler(UserManager<IdentityUser> _userManager, IConfiguration _configuration)
        {
            this._userManager = _userManager;
            this._configuration = _configuration;
        }
        public async Task<object> Handle(GenerateUserJWTQuery request, CancellationToken cancellationToken)
        {
            var userInDb = await _userManager.FindByEmailAsync(request.AuthenticateUserDTO.Email);

            if (userInDb == null || !await _userManager.CheckPasswordAsync(userInDb, request.AuthenticateUserDTO.Password))
                return null;


            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value.ToString());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = userInDb.Email,
                Subject = new ClaimsIdentity(await getUserClaimsAsync(userInDb)),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        private async Task<List<Claim>> getUserClaimsAsync(IdentityUser userInDb)
        {
            var userRoles = await _userManager.GetRolesAsync(userInDb);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Id", userInDb.Id));
            claims.Add(new Claim(ClaimTypes.Name, userInDb.UserName));
            for (int i = 0; i < userRoles.Count; i++)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRoles[i]));
            }
            return claims;
        }
    }
}

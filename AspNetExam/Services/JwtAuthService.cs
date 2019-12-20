using AspNetExam.DataAccess;
using AspNetExam.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetExam.Services
{
    public class JwtAuthService : IAuthService
    {
        private readonly DataContext context;
        private readonly string jwtSecret;

        public JwtAuthService(DataContext context, IOptions<JwtOptions> jwtSecret)
        {
            this.context = context;
            this.jwtSecret = jwtSecret.Value.Secret;
        }

        public async Task<string> Authenticate(string login, string password)
        {
            var supposedUser = await context.Users.SingleOrDefaultAsync(user => user.Login == login && user.Password == password);

            if (supposedUser == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, supposedUser.Login)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

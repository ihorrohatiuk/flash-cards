using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlashCards.Core.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FlashCards.Infrastructure.Security;

public class JwtProvider(IOptions<JwtOptions> jwtOptions)
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string GenerateJwtToken(User user)
    {
        Claim[] claims = 
        [
            new("userId", user.Id.ToString()), 
            new("email", user.Email),
            new("role", user.Role)
        ];
        
        var signinCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signinCredentials,
            expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpireHours));
        
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        
        return tokenValue;
    }
}
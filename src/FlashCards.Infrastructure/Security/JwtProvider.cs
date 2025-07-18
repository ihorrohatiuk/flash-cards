﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.DataModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FlashCards.Infrastructure.Security;

public class JwtProvider(IOptions<JwtOptions> jwtOptions)
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string GenerateJwtToken(UserEntity userEntity)
    {
        Claim[] claims = 
        [
            new(JwtClaims.UserId, userEntity.Id.ToString()),
            new(JwtClaims.Email, userEntity.Email),
            new (JwtClaims.FirstName, userEntity.FirstName),
            new (JwtClaims.LastName, userEntity.LastName),
            new(JwtClaims.Role, userEntity.Role)
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Services.Types;
using Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApiUser> _userManager;

    public AuthService(UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<List<Claim>> GetAuthClaims(ApiUser user)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.UserName),
            new(AuthConstants.UserIdClaimType, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
        };

        var userRoles = await _userManager.GetRolesAsync(user);

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        return authClaims;
    }

    public JwtSecurityToken GetAuthToken(List<Claim> authClaims)
    {
        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            null,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: CreateSigningCredentials()
        );

        return token;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}
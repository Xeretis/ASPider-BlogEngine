using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Data.Entities;

namespace Auth.Services.Types;

public interface IAuthService
{
    Task<List<Claim>> GetAuthClaims(ApiUser user);
    JwtSecurityToken GetAuthToken(List<Claim> authClaims);
}
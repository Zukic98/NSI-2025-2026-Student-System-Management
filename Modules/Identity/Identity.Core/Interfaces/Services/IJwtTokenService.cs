using System.Security.Claims;
using Identity.Core.Entities;
using Identity.Core.Models;

namespace Identity.Core.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(TokenClaims claims);
    string GenerateRefreshToken();
    RefreshToken CreateRefreshToken(Guid userId);
    ClaimsPrincipal? ValidateAccessToken(string token);
}

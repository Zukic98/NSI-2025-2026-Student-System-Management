using Identity.Core.Models;
using Identity.Core.Entities;

namespace Identity.Core.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResult> AuthenticateAsync(string email, string password, string ipAddress, string userAgent, CancellationToken cancellationToken = default);
    Task<AuthResult> RefreshAuthenticationAsync(string refreshToken, string ipAddress, string userAgent, CancellationToken cancellationToken = default);
    Task RevokeAuthenticationAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<PublicKeyInfo> GetPublicKeyInfoAsync();
    Task<AuthResult> IssueTokensForUserAsync(Guid userId);
    Task<User?> AuthenticatePasswordOnlyAsync(string email, string password);
}

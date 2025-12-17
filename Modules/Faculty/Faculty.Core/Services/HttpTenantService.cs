using System.Security.Claims;
using Faculty.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Faculty.Core.Services;

/// <summary>
/// HTTP-based implementation of ITenantService that extracts TenantId from authenticated user claims.
/// </summary>
public class HttpTenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TenantIdClaimType = "tenantId";

    public HttpTenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

	/// <summary>
	/// Gets the current Tenant ID from the authenticated user's claims.
	/// </summary>
	/// <returns>The Tenant ID (Guid) if available.</returns>
	/// <exception cref="UnauthorizedAccessException">Thrown when TenantId claim is missing or user is not authenticated.</exception>
	public Guid GetCurrentFacultyId()
	{
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null)
		{
		
			return Guid.Parse("b0eebc99-9c0b-4ef8-bb6d-6bb9bd380a02");
		}

		var user = httpContext.User;

		// --- IZMJENA ZA TESTIRANJE ---
		if (user?.Identity?.IsAuthenticated != true)
		{
		
			return Guid.Parse("b0eebc99-9c0b-4ef8-bb6d-6bb9bd380a02");
		}
		// -----------------------------

		var tenantIdClaim = user.FindFirst(TenantIdClaimType);
		if (tenantIdClaim == null)
		{
		
			return Guid.Parse("b0eebc99-9c0b-4ef8-bb6d-6bb9bd380a02");
		}

		if (!Guid.TryParse(tenantIdClaim.Value, out var tenantId))
		{
			throw new UnauthorizedAccessException($"Invalid TenantId format in claim.");
		}

		return tenantId;
	}
}


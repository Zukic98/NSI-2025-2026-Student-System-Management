using Identity.API.DTO.Auth;
using Identity.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Core.Models;
using Identity.Core.Entities;
using Identity.Application.DTO;
using Identity.Application.Interfaces;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITwoFactorAuthService _twoFactorService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ITwoFactorAuthService twoFactorService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _twoFactorService = twoFactorService;
        _logger = logger;
    }


    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1) Validate email + password only (no tokens issued here)
            var user = await _authService.AuthenticatePasswordOnlyAsync(request.Email, request.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            // 2) If user has NEVER set up 2FA - start setup flow
            if (!user.TwoFactorEnabled)
            {
                return Ok(new { requires2FASetup = true, userId = user.Id });
            }

            // 3) If user has 2FA enabled - require a 2FA code (no tokens yet!)
            return Ok(new { requires2FA = true, userId = user.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpPost("verify-2fa")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFAConfirmRequest dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // STEP 1: Validate TOTP code
            var verification = await _twoFactorService.VerifyLoginAsync(dto.UserId, dto.Code);

            if (!verification.Success)
            {
                return verification.Error switch
                {
                    TwoFAVerificationError.InvalidCode =>
                        Unauthorized(new { message = "Invalid 2FA code" }),

                    TwoFAVerificationError.RateLimited =>
                        StatusCode(StatusCodes.Status429TooManyRequests,
                            new { message = "Too many attempts. Please wait and try again." }),

                    TwoFAVerificationError.UserNotFound =>
                        NotFound(new { message = "User not found" }),

                    _ => BadRequest(new { message = "2FA verification failed" })
                };
            }

            // STEP 2: Issue JWT + Refresh Token (Application Layer)
            var userId = Guid.Parse(dto.UserId);
            var authResult = await _authService.IssueTokensForUserAsync(userId);

            // STEP 3: Set refresh token cookie (HTTP-only)
            HttpContext.Response.Cookies.Append(
                "refreshToken",
                authResult.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = authResult.ExpiresAt
                });

            // STEP 4: Return full authentication response
            return Ok(new
            {
                accessToken = authResult.AccessToken,
                tokenType = "Bearer",
                expiresOn = authResult.ExpiresAt,

                userId = authResult.UserId,
                email = authResult.Email,
                role = authResult.Role,
                tenantId = authResult.TenantId,
                fullName = authResult.FullName
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "2FA verification failed for user {UserId}", dto.UserId);
            return StatusCode(500, new { message = "An error occurred during 2FA verification" });
        }
    }



    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> RefreshToken()
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new { message = "Refresh token is required" });
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString() ?? "unknown";

            var result = await _authService.RefreshAuthenticationAsync(
                refreshToken,
                ipAddress,
                userAgent);

            // Set HTTP-only cookie for new refresh token
            HttpContext.Response.Cookies.Append(
                "refreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.ExpiresAt
                });

            // Map domain model to DTO
            var response = new LoginResponseDto
            {
                AccessToken = result.AccessToken,
                TokenType = "Bearer"
            };

            return Ok(response);

        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Token refresh failed");
            return Unauthorized(new { message = "Invalid or expired refresh token" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, new { message = "An error occurred during token refresh" });
        }
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new { message = "Refresh token is required" });
            }

            await _authService.RevokeAuthenticationAsync(refreshToken);

            // Clear the refresh token cookie
            HttpContext.Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Successfully logged out" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, new { message = "An error occurred during logout" });
        }
    }
}
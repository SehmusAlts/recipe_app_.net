using RecipeApp.Application.DTOs.Auth;

namespace RecipeApp.Application.Interfaces;

/// <summary>
/// Authentication service interface.
/// Follows Interface Segregation Principle.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets user ID from JWT token
    /// </summary>
    Guid GetUserIdFromToken(string token);
}

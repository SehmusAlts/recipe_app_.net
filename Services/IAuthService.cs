using RecipeApp.Web.Models.ViewModels;

namespace RecipeApp.Web.Services;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(string email, string password);
    Task<AuthResponse?> RegisterAsync(RegisterViewModel model);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetTokenAsync();
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

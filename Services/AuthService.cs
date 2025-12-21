using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RecipeApp.Web.Models.ViewModels;

namespace RecipeApp.Web.Services;

public class AuthService : IAuthService
{
    private readonly IApiService _apiService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private const string TokenKey = "auth_token";
    private const string UserEmailKey = "user_email";
    private const string UserNameKey = "user_name";

    public AuthService(
        IApiService apiService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger)
    {
        _apiService = apiService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<AuthResponse?> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _apiService.PostAsync<AuthResponse>("api/auth/login", new
            {
                email,
                password
            });

            if (response != null)
            {
                await StoreAuthDataAsync(response);
                _apiService.SetAuthToken(response.Token);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for {Email}", email);
            return null;
        }
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterViewModel model)
    {
        try
        {
            var response = await _apiService.PostAsync<AuthResponse>("api/auth/register", new
            {
                email = model.Email,
                password = model.Password,
                confirmPassword = model.ConfirmPassword,
                firstName = model.FirstName,
                lastName = model.LastName
            });

            if (response != null)
            {
                await StoreAuthDataAsync(response);
                _apiService.SetAuthToken(response.Token);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed for {Email}", model.Email);
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            session.Remove(TokenKey);
            session.Remove(UserEmailKey);
            session.Remove(UserNameKey);
        }

        _apiService.ClearAuthToken();
        await Task.CompletedTask;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            var token = session.GetString(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetAuthToken(token);
                return token;
            }
        }

        return await Task.FromResult<string?>(null);
    }

    private async Task StoreAuthDataAsync(AuthResponse response)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            session.SetString(TokenKey, response.Token);
            session.SetString(UserEmailKey, response.Email);
            session.SetString(UserNameKey, $"{response.FirstName} {response.LastName}");
        }

        await Task.CompletedTask;
    }
}

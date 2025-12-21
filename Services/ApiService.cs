using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RecipeApp.Web.Services;

public class ApiService : IApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ApiService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private string? _authToken;

    public ApiService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public void SetAuthToken(string token)
    {
        _authToken = token;
        _logger.LogInformation("Auth token stored: {TokenPreview}...", 
            string.IsNullOrEmpty(token) ? "NULL" : $"Bearer {token.Substring(0, Math.Min(20, token.Length))}");
    }

    public void ClearAuthToken()
    {
        _authToken = null;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        if (!string.IsNullOrEmpty(_authToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            _logger.LogInformation("Authorization header added to request: Bearer {TokenPreview}...", 
                _authToken.Substring(0, Math.Min(20, _authToken.Length)));
        }
        return client;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var client = CreateClient();
            var response = await client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GET request to {Endpoint} failed with status {StatusCode}", endpoint, response.StatusCode);
                return default;
            }

            // UTF-8 encoding için byte array'den okuyoruz
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var content = Encoding.UTF8.GetString(bytes);
            
            var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while GET request to {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<T?> PostAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            var client = CreateClient();
            
            var json = data != null ? JsonSerializer.Serialize(data) : string.Empty;
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("POST request to {Endpoint} failed with status {StatusCode}: {Error}",
                    endpoint, response.StatusCode, errorContent);
                return default;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while POST request to {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("PUT request to {Endpoint} failed with status {StatusCode}", endpoint, response.StatusCode);
                return default;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while PUT request to {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var client = CreateClient();
            var response = await client.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while DELETE request to {Endpoint}", endpoint);
            throw;
        }
    }
}

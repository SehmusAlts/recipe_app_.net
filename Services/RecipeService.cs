using RecipeApp.Web.Models;

namespace RecipeApp.Web.Services;

public class RecipeService : IRecipeService
{
    private readonly IApiService _apiService;
    private readonly ILogger<RecipeService> _logger;

    public RecipeService(IApiService apiService, ILogger<RecipeService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<PagedResultDto<RecipeDto>?> GetRecipesAsync(int pageNumber = 1, int pageSize = 12, RecipeCategory? category = null)
    {
        var categoryParam = category.HasValue ? $"&category={category.Value}" : string.Empty;
        var endpoint = $"api/recipes?pageNumber={pageNumber}&pageSize={pageSize}{categoryParam}";
        return await _apiService.GetAsync<PagedResultDto<RecipeDto>>(endpoint);
    }

    public async Task<RecipeDto?> GetRecipeByIdAsync(Guid id)
    {
        return await _apiService.GetAsync<RecipeDto>($"api/recipes/{id}");
    }

    public async Task<RecipeDto?> CreateRecipeAsync(CreateRecipeRequest request)
    {
        return await _apiService.PostAsync<RecipeDto>("api/recipes", request);
    }

    public async Task<RecipeDto?> UpdateRecipeAsync(Guid id, UpdateRecipeRequest request)
    {
        return await _apiService.PutAsync<RecipeDto>($"api/recipes/{id}", request);
    }

    public async Task<bool> DeleteRecipeAsync(Guid id)
    {
        return await _apiService.DeleteAsync($"api/recipes/{id}");
    }

    public async Task<PagedResultDto<RecipeDto>?> GetFavoriteRecipesAsync(int pageNumber = 1, int pageSize = 12)
    {
        var endpoint = $"api/recipes/favorites?pageNumber={pageNumber}&pageSize={pageSize}";
        return await _apiService.GetAsync<PagedResultDto<RecipeDto>>(endpoint);
    }

    public async Task<bool> AddToFavoritesAsync(Guid recipeId)
    {
        try
        {
            var result = await _apiService.PostAsync<object>($"api/recipes/{recipeId}/favorite");
            return result != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveFromFavoritesAsync(Guid recipeId)
    {
        return await _apiService.DeleteAsync($"api/recipes/{recipeId}/favorite");
    }

    public async Task<bool> SyncExternalRecipesAsync()
    {
        try
        {
            var result = await _apiService.PostAsync<object>("api/recipes/sync");
            return result != null;
        }
        catch
        {
            return false;
        }
    }
}

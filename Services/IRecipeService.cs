using RecipeApp.Web.Models;

namespace RecipeApp.Web.Services;

public interface IRecipeService
{
    Task<PagedResultDto<RecipeDto>?> GetRecipesAsync(int pageNumber = 1, int pageSize = 12, RecipeCategory? category = null);
    Task<RecipeDto?> GetRecipeByIdAsync(Guid id);
    Task<RecipeDto?> CreateRecipeAsync(CreateRecipeRequest request);
    Task<RecipeDto?> UpdateRecipeAsync(Guid id, UpdateRecipeRequest request);
    Task<bool> DeleteRecipeAsync(Guid id);
    Task<PagedResultDto<RecipeDto>?> GetFavoriteRecipesAsync(int pageNumber = 1, int pageSize = 12);
    Task<bool> AddToFavoritesAsync(Guid recipeId);
    Task<bool> RemoveFromFavoritesAsync(Guid recipeId);
    Task<bool> SyncExternalRecipesAsync();
}

public class CreateRecipeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Ingredients { get; set; } = new();
    public string Instructions { get; set; } = string.Empty;
    public int Category { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public int Servings { get; set; }
    public string? ImageUrl { get; set; }
}

public class UpdateRecipeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Ingredients { get; set; } = new();
    public string Instructions { get; set; } = string.Empty;
    public int Category { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public int Servings { get; set; }
    public string? ImageUrl { get; set; }
}

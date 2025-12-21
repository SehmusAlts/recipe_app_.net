using RecipeApp.Application.DTOs.Recipe;

namespace RecipeApp.Application.Interfaces;

/// <summary>
/// External recipe API service interface.
/// Follows Interface Segregation and Dependency Inversion Principles.
/// </summary>
public interface IExternalRecipeApiService
{
    /// <summary>
    /// Fetches recipes from external API (DummyJSON)
    /// </summary>
    Task<List<RecipeDto>> FetchRecipesAsync(int limit = 50, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetches a single recipe from external API
    /// </summary>
    Task<RecipeDto?> FetchRecipeByIdAsync(int externalId, CancellationToken cancellationToken = default);
}

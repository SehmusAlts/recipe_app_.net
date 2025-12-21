using RecipeApp.Application.DTOs.Common;
using RecipeApp.Application.DTOs.Recipe;
using RecipeApp.Domain.Enums;

namespace RecipeApp.Application.Interfaces;

/// <summary>
/// Recipe service interface.
/// Follows Interface Segregation Principle.
/// </summary>
public interface IRecipeService
{
    /// <summary>
    /// Gets all recipes with pagination
    /// </summary>
    Task<PagedResultDto<RecipeDto>> GetAllRecipesAsync(
        int pageNumber,
        int pageSize,
        RecipeCategory? category = null,
        Guid? userId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a recipe by ID
    /// </summary>
    Task<RecipeDto> GetRecipeByIdAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user recipe
    /// </summary>
    Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto dto, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user recipe
    /// </summary>
    Task<RecipeDto> UpdateRecipeAsync(UpdateRecipeDto dto, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a user recipe
    /// </summary>
    Task DeleteRecipeAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets user's favorite recipes
    /// </summary>
    Task<PagedResultDto<RecipeDto>> GetFavoriteRecipesAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds recipe to favorites
    /// </summary>
    Task AddToFavoritesAsync(Guid recipeId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes recipe from favorites
    /// </summary>
    Task RemoveFromFavoritesAsync(Guid recipeId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Syncs recipes from external API
    /// </summary>
    Task SyncExternalRecipesAsync(CancellationToken cancellationToken = default);
}

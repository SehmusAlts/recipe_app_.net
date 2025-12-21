using RecipeApp.Application.DTOs.Rating;

namespace RecipeApp.Application.Interfaces;

/// <summary>
/// Rating service interface.
/// Follows Interface Segregation Principle.
/// </summary>
public interface IRatingService
{
    /// <summary>
    /// Creates or updates a rating for a recipe
    /// </summary>
    Task<RatingDto> CreateOrUpdateRatingAsync(
        CreateRatingDto dto,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all ratings for a recipe
    /// </summary>
    Task<List<RatingDto>> GetRecipeRatingsAsync(Guid recipeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets user's rating for a recipe
    /// </summary>
    Task<RatingDto?> GetUserRatingForRecipeAsync(
        Guid recipeId,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a rating
    /// </summary>
    Task DeleteRatingAsync(Guid recipeId, Guid userId, CancellationToken cancellationToken = default);
}

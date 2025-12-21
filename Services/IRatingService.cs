using RecipeApp.Web.Models;

namespace RecipeApp.Web.Services;

public interface IRatingService
{
    Task<List<RatingDto>?> GetRecipeRatingsAsync(Guid recipeId);
    Task<RatingDto?> GetMyRatingAsync(Guid recipeId);
    Task<RatingDto?> CreateOrUpdateRatingAsync(CreateRatingRequest request);
    Task<bool> DeleteRatingAsync(Guid id);
}

public class CreateRatingRequest
{
    public Guid RecipeId { get; set; }
    public int Value { get; set; }
    public string? Comment { get; set; }
}

using RecipeApp.Web.Models;

namespace RecipeApp.Web.Services;

public class RatingService : IRatingService
{
    private readonly IApiService _apiService;
    private readonly ILogger<RatingService> _logger;

    public RatingService(IApiService apiService, ILogger<RatingService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<List<RatingDto>?> GetRecipeRatingsAsync(Guid recipeId)
    {
        return await _apiService.GetAsync<List<RatingDto>>($"api/ratings/recipe/{recipeId}");
    }

    public async Task<RatingDto?> GetMyRatingAsync(Guid recipeId)
    {
        return await _apiService.GetAsync<RatingDto>($"api/ratings/recipe/{recipeId}/my-rating");
    }

    public async Task<RatingDto?> CreateOrUpdateRatingAsync(CreateRatingRequest request)
    {
        return await _apiService.PostAsync<RatingDto>("api/ratings", request);
    }

    public async Task<bool> DeleteRatingAsync(Guid id)
    {
        return await _apiService.DeleteAsync($"api/ratings/{id}");
    }
}

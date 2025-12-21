using Microsoft.AspNetCore.Mvc;
using RecipeApp.Web.Models;
using RecipeApp.Web.Services;

namespace RecipeApp.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingController : Controller
{
    private readonly IRatingService _ratingService;
    private readonly IAuthService _authService;
    private readonly ILogger<RatingController> _logger;

    public RatingController(
        IRatingService ratingService,
        IAuthService authService,
        ILogger<RatingController> logger)
    {
        _ratingService = ratingService;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Get all ratings for a recipe
    /// </summary>
    [HttpGet("recipe/{recipeId}")]
    public async Task<IActionResult> GetRecipeRatings(Guid recipeId)
    {
        try
        {
            var ratings = await _ratingService.GetRecipeRatingsAsync(recipeId);
            if (ratings == null)
            {
                return NotFound(new { message = "Tarif bulunamadı" });
            }
            return Ok(ratings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting ratings for recipe {RecipeId}", recipeId);
            return StatusCode(500, new { message = "Değerlendirmeler yüklenirken hata oluştu" });
        }
    }

    /// <summary>
    /// Get user's rating for a recipe
    /// </summary>
    [HttpGet("recipe/{recipeId}/my-rating")]
    public async Task<IActionResult> GetMyRating(Guid recipeId)
    {
        try
        {
            var token = await _authService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Lütfen giriş yapın" });
            }

            var rating = await _ratingService.GetMyRatingAsync(recipeId);
            if (rating == null)
            {
                return NotFound(new { message = "Bu tarifi henüz değerlendirmediniz" });
            }

            return Ok(rating);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user rating for recipe {RecipeId}", recipeId);
            return StatusCode(500, new { message = "Değerlendirme yüklenirken hata oluştu" });
        }
    }

    /// <summary>
    /// Create or update a rating
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateRating([FromBody] CreateRatingRequest request)
    {
        try
        {
            var token = await _authService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Lütfen giriş yapın" });
            }

            _logger.LogInformation("Creating/updating rating for recipe {RecipeId} with value {Value}", 
                request.RecipeId, request.Value);

            var rating = await _ratingService.CreateOrUpdateRatingAsync(request);
            if (rating == null)
            {
                return BadRequest(new { message = "Değerlendirme kaydedilemedi" });
            }

            return Ok(rating);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating/updating rating for recipe {RecipeId}", request.RecipeId);
            return StatusCode(500, new { message = "Değerlendirme kaydedilirken hata oluştu" });
        }
    }

    /// <summary>
    /// Delete a rating
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRating(Guid id)
    {
        try
        {
            var token = await _authService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Lütfen giriş yapın" });
            }

            var result = await _ratingService.DeleteRatingAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Değerlendirme bulunamadı" });
            }

            return Ok(new { message = "Değerlendirme silindi" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting rating {RatingId}", id);
            return StatusCode(500, new { message = "Değerlendirme silinirken hata oluştu" });
        }
    }
}

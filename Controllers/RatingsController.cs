using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.Application.DTOs.Rating;
using RecipeApp.Application.Interfaces;
using RecipeApp.Domain.Exceptions;
using System.Security.Claims;

namespace RecipeApp.API.Controllers;

/// <summary>
/// Ratings controller.
/// Handles recipe rating operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
    }

    /// <summary>
    /// Get all ratings for a recipe
    /// </summary>
    [HttpGet("recipe/{recipeId}")]
    [ProducesResponseType(typeof(List<RatingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RatingDto>>> GetRecipeRatings(Guid recipeId)
    {
        try
        {
            var ratings = await _ratingService.GetRecipeRatingsAsync(recipeId);
            return Ok(ratings);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get user's rating for a recipe
    /// </summary>
    [Authorize]
    [HttpGet("recipe/{recipeId}/my-rating")]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RatingDto>> GetMyRating(Guid recipeId)
    {
        var userId = GetUserIdFromToken();
        var rating = await _ratingService.GetUserRatingForRecipeAsync(recipeId, userId);

        if (rating == null)
        {
            return NotFound(new { message = "You haven't rated this recipe yet" });
        }

        return Ok(rating);
    }

    /// <summary>
    /// Create or update a rating
    /// </summary>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RatingDto>> CreateOrUpdateRating([FromBody] CreateRatingDto dto)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var rating = await _ratingService.CreateOrUpdateRatingAsync(dto, userId);
            return Ok(rating);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    /// <summary>
    /// Delete a rating
    /// </summary>
    [Authorize]
    [HttpDelete("recipe/{recipeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating(Guid recipeId)
    {
        try
        {
            var userId = GetUserIdFromToken();
            await _ratingService.DeleteRatingAsync(recipeId, userId);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid token");
        }
        return userId;
    }
}

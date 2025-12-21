using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.Application.DTOs.Common;
using RecipeApp.Application.DTOs.Recipe;
using RecipeApp.Application.Interfaces;
using RecipeApp.Domain.Enums;
using RecipeApp.Domain.Exceptions;
using System.Security.Claims;

namespace RecipeApp.API.Controllers;

/// <summary>
/// Recipes controller.
/// Handles recipe CRUD operations and favorites.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService ?? throw new ArgumentNullException(nameof(recipeService));
    }

    /// <summary>
    /// Get all recipes with pagination and filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<RecipeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<RecipeDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] RecipeCategory? category = null)
    {
        var userId = GetUserIdFromToken();
        var result = await _recipeService.GetAllRecipesAsync(pageNumber, pageSize, category, userId);
        return Ok(result);
    }

    /// <summary>
    /// Get recipe by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RecipeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeDto>> GetById(Guid id)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var recipe = await _recipeService.GetRecipeByIdAsync(id, userId);
            return Ok(recipe);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new recipe
    /// </summary>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(RecipeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RecipeDto>> Create([FromBody] CreateRecipeDto dto)
    {
        try
        {
            var userId = GetUserIdFromToken()!.Value;
            var recipe = await _recipeService.CreateRecipeAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, recipe);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    /// <summary>
    /// Update an existing recipe
    /// </summary>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RecipeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeDto>> Update(Guid id, [FromBody] UpdateRecipeDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var userId = GetUserIdFromToken()!.Value;
            var recipe = await _recipeService.UpdateRecipeAsync(dto, userId);
            return Ok(recipe);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    /// <summary>
    /// Delete a recipe
    /// </summary>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = GetUserIdFromToken()!.Value;
            await _recipeService.DeleteRecipeAsync(id, userId);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    /// <summary>
    /// Get user's favorite recipes
    /// </summary>
    [Authorize]
    [HttpGet("favorites")]
    [ProducesResponseType(typeof(PagedResultDto<RecipeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResultDto<RecipeDto>>> GetFavorites(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetUserIdFromToken()!.Value;
        var result = await _recipeService.GetFavoriteRecipesAsync(userId, pageNumber, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Add recipe to favorites
    /// </summary>
    [Authorize]
    [HttpPost("{id}/favorite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddToFavorites(Guid id)
    {
        try
        {
            var userId = GetUserIdFromToken()!.Value;
            await _recipeService.AddToFavoritesAsync(id, userId);
            return Ok(new { message = "Recipe added to favorites" });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove recipe from favorites
    /// </summary>
    [Authorize]
    [HttpDelete("{id}/favorite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromFavorites(Guid id)
    {
        try
        {
            var userId = GetUserIdFromToken()!.Value;
            await _recipeService.RemoveFromFavoritesAsync(id, userId);
            return Ok(new { message = "Recipe removed from favorites" });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Sync recipes from external API (DummyJSON)
    /// </summary>
    [HttpPost("sync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SyncExternalRecipes()
    {
        await _recipeService.SyncExternalRecipesAsync();
        return Ok(new { message = "Recipes synced successfully" });
    }

    private Guid? GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null && Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}

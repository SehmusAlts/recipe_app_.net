using RecipeApp.Domain.Enums;

namespace RecipeApp.Application.DTOs.Rating;

/// <summary>
/// DTO for creating or updating a recipe rating.
/// </summary>
public class CreateRatingDto
{
    public Guid RecipeId { get; set; }
    public RatingValue Value { get; set; }
    public string? Comment { get; set; }
}

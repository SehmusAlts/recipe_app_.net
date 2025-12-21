using RecipeApp.Domain.Enums;

namespace RecipeApp.Application.DTOs.Rating;

/// <summary>
/// DTO for rating display.
/// </summary>
public class RatingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid RecipeId { get; set; }
    public RatingValue Value { get; set; }
    public string? Comment { get; set; }
    public DateTime RatedAt { get; set; }
}

using RecipeApp.Domain.Common;
using RecipeApp.Domain.Enums;

namespace RecipeApp.Domain.Entities;

/// <summary>
/// Represents a user's rating for a recipe.
/// Follows Single Responsibility Principle.
/// </summary>
public class Rating : BaseEntity
{
    /// <summary>
    /// User who rated the recipe
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property: User
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Recipe that was rated
    /// </summary>
    public Guid RecipeId { get; set; }

    /// <summary>
    /// Navigation property: Recipe
    /// </summary>
    public Recipe Recipe { get; set; } = null!;

    /// <summary>
    /// Rating value (1-5 stars)
    /// </summary>
    public RatingValue Value { get; set; }

    /// <summary>
    /// Optional review comment
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Date when the rating was given
    /// </summary>
    public DateTime RatedAt { get; set; } = DateTime.UtcNow;
}

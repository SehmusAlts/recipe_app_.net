using RecipeApp.Domain.Common;

namespace RecipeApp.Domain.Entities;

/// <summary>
/// Represents a user's favorite recipe.
/// Join entity between User and Recipe.
/// Follows Single Responsibility Principle.
/// </summary>
public class Favorite : BaseEntity
{
    /// <summary>
    /// User who favorited the recipe
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property: User
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Recipe that was favorited
    /// </summary>
    public Guid RecipeId { get; set; }

    /// <summary>
    /// Navigation property: Recipe
    /// </summary>
    public Recipe Recipe { get; set; } = null!;

    /// <summary>
    /// Date when the recipe was added to favorites
    /// </summary>
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}

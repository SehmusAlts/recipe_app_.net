using RecipeApp.Domain.Common;
using RecipeApp.Domain.Enums;

namespace RecipeApp.Domain.Entities;

/// <summary>
/// Represents a recipe in the system.
/// Can be sourced from external API or created by users.
/// Follows Single Responsibility Principle by handling only recipe-related data.
/// </summary>
public class Recipe : BaseEntity
{
    /// <summary>
    /// Recipe name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Recipe description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of ingredients (stored as JSON or delimited string)
    /// </summary>
    public string Ingredients { get; set; } = string.Empty;

    /// <summary>
    /// Preparation instructions
    /// </summary>
    public string Instructions { get; set; } = string.Empty;

    /// <summary>
    /// Recipe category
    /// </summary>
    public RecipeCategory Category { get; set; }

    /// <summary>
    /// Preparation time in minutes
    /// </summary>
    public int PreparationTimeMinutes { get; set; }

    /// <summary>
    /// Cooking time in minutes
    /// </summary>
    public int CookingTimeMinutes { get; set; }

    /// <summary>
    /// Number of servings
    /// </summary>
    public int Servings { get; set; }

    /// <summary>
    /// Image URL
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// External API recipe ID (if sourced from DummyJSON)
    /// </summary>
    public int? ExternalApiId { get; set; }

    /// <summary>
    /// Indicates if recipe is from external API
    /// </summary>
    public bool IsFromExternalApi { get; set; }

    /// <summary>
    /// User who created this recipe (null if from external API)
    /// </summary>
    public Guid? CreatedByUserId { get; set; }

    /// <summary>
    /// Navigation property: Creator user
    /// </summary>
    public User? CreatedByUser { get; set; }

    /// <summary>
    /// Navigation property: Users who favorited this recipe
    /// </summary>
    public ICollection<Favorite> FavoritedBy { get; set; } = new List<Favorite>();

    /// <summary>
    /// Navigation property: Ratings for this recipe
    /// </summary>
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    /// <summary>
    /// Computed property: Average rating
    /// </summary>
    public double AverageRating => Ratings.Any()
        ? Ratings.Average(r => (int)r.Value)
        : 0;

    /// <summary>
    /// Computed property: Total ratings count
    /// </summary>
    public int RatingsCount => Ratings.Count;
}

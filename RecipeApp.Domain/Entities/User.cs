using RecipeApp.Domain.Common;

namespace RecipeApp.Domain.Entities;

/// <summary>
/// Represents a user in the system.
/// Follows Single Responsibility Principle by handling only user-related data.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// User's email address (used for authentication)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// User's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's full name (computed property)
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Navigation property: User's favorite recipes
    /// </summary>
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    /// <summary>
    /// Navigation property: Ratings given by the user
    /// </summary>
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    /// <summary>
    /// Navigation property: Recipes created by the user
    /// </summary>
    public ICollection<Recipe> CreatedRecipes { get; set; } = new List<Recipe>();
}

using RecipeApp.Domain.Enums;

namespace RecipeApp.Application.DTOs.Recipe;

/// <summary>
/// DTO for recipe display.
/// </summary>
public class RecipeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Ingredients { get; set; } = new();
    public string Instructions { get; set; } = string.Empty;
    public RecipeCategory Category { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public int Servings { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsFromExternalApi { get; set; }
    public double AverageRating { get; set; }
    public int RatingsCount { get; set; }
    public bool IsFavorited { get; set; }
    public DateTime CreatedAt { get; set; }
}

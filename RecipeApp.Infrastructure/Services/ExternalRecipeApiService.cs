using System.Net.Http.Json;
using RecipeApp.Application.DTOs.Recipe;
using RecipeApp.Application.Interfaces;
using RecipeApp.Domain.Enums;

namespace RecipeApp.Infrastructure.Services;

/// <summary>
/// External recipe API service implementation (DummyJSON).
/// Follows Single Responsibility Principle - handles only external API communication.
/// </summary>
public class ExternalRecipeApiService : IExternalRecipeApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://dummyjson.com";

    public ExternalRecipeApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<List<RecipeDto>> FetchRecipesAsync(int limit = 50, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<DummyJsonRecipesResponse>(
                $"/recipes?limit={limit}",
                cancellationToken);

            if (response?.Recipes == null)
            {
                return new List<RecipeDto>();
            }

            return response.Recipes.Select(MapToRecipeDto).ToList();
        }
        catch (Exception ex)
        {
            // Log error and return empty list
            Console.WriteLine($"Error fetching recipes from DummyJSON: {ex.Message}");
            return new List<RecipeDto>();
        }
    }

    public async Task<RecipeDto?> FetchRecipeByIdAsync(int externalId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<DummyJsonRecipe>(
                $"/recipes/{externalId}",
                cancellationToken);

            return response != null ? MapToRecipeDto(response) : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching recipe {externalId} from DummyJSON: {ex.Message}");
            return null;
        }
    }

    private static RecipeDto MapToRecipeDto(DummyJsonRecipe source)
    {
        return new RecipeDto
        {
            Id = Guid.NewGuid(), // Will be replaced when saving to DB
            Name = source.Name ?? "Unknown Recipe",
            Description = source.Instructions?.FirstOrDefault() ?? "No description available",
            Ingredients = source.Ingredients ?? new List<string>(),
            Instructions = string.Join("\n", source.Instructions ?? new List<string>()),
            Category = MapCategory(source.MealType?.FirstOrDefault()),
            PreparationTimeMinutes = source.PrepTimeMinutes,
            CookingTimeMinutes = source.CookTimeMinutes,
            Servings = source.Servings,
            ImageUrl = source.Image,
            IsFromExternalApi = true,
            AverageRating = source.Rating,
            RatingsCount = source.ReviewCount,
            IsFavorited = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static RecipeCategory MapCategory(string? mealType)
    {
        return mealType?.ToLower() switch
        {
            "breakfast" => RecipeCategory.Kahvalti,
            "lunch" => RecipeCategory.AnaYemek,
            "dinner" => RecipeCategory.AnaYemek,
            "dessert" => RecipeCategory.Tatli,
            "snack" => RecipeCategory.Atistirmaliklar,
            "appetizer" => RecipeCategory.Mezeler,
            "beverage" => RecipeCategory.Icecek,
            _ => RecipeCategory.Diger
        };
    }

    // DummyJSON response models
    private class DummyJsonRecipesResponse
    {
        public List<DummyJsonRecipe>? Recipes { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }

    private class DummyJsonRecipe
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string>? Ingredients { get; set; }
        public List<string>? Instructions { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public string? Difficulty { get; set; }
        public string? Cuisine { get; set; }
        public int CaloriesPerServing { get; set; }
        public List<string>? Tags { get; set; }
        public int UserId { get; set; }
        public string? Image { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public List<string>? MealType { get; set; }
    }
}

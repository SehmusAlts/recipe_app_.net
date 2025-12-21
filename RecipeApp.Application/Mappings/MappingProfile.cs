using AutoMapper;
using RecipeApp.Application.DTOs.Recipe;
using RecipeApp.Application.DTOs.Rating;
using RecipeApp.Domain.Entities;
using System.Text.Json;

namespace RecipeApp.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity-DTO mappings.
/// Follows Single Responsibility Principle.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Recipe mappings
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                DeserializeIngredients(src.Ingredients)))
            .ForMember(dest => dest.IsFavorited, opt => opt.Ignore()); // Set separately

        CreateMap<CreateRecipeDto, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                SerializeIngredients(src.Ingredients)))
            .ForMember(dest => dest.IsFromExternalApi, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.ExternalApiId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.FavoritedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Ratings, opt => opt.Ignore());

        CreateMap<UpdateRecipeDto, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                SerializeIngredients(src.Ingredients)))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.IsFromExternalApi, opt => opt.Ignore())
            .ForMember(dest => dest.ExternalApiId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.FavoritedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Ratings, opt => opt.Ignore());

        // Rating mappings
        CreateMap<Rating, RatingDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));

        CreateMap<CreateRatingDto, Rating>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Recipe, opt => opt.Ignore())
            .ForMember(dest => dest.RatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }

    private static List<string> DeserializeIngredients(string ingredients)
    {
        return JsonSerializer.Deserialize<List<string>>(ingredients, (JsonSerializerOptions?)null) ?? new List<string>();
    }

    private static string SerializeIngredients(List<string> ingredients)
    {
        return JsonSerializer.Serialize(ingredients, (JsonSerializerOptions?)null);
    }
}

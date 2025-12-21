using AutoMapper;
using RecipeApp.Application.DTOs.Common;
using RecipeApp.Application.DTOs.Recipe;
using RecipeApp.Application.Interfaces;
using RecipeApp.Domain.Entities;
using RecipeApp.Domain.Enums;
using RecipeApp.Domain.Exceptions;
using RecipeApp.Domain.Interfaces;

namespace RecipeApp.Application.Services;

/// <summary>
/// Recipe service implementation.
/// Follows Single Responsibility Principle - handles only recipe business logic.
/// </summary>
public class RecipeService : IRecipeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExternalRecipeApiService _externalApiService;

    public RecipeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IExternalRecipeApiService externalApiService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _externalApiService = externalApiService ?? throw new ArgumentNullException(nameof(externalApiService));
    }

    public async Task<PagedResultDto<RecipeDto>> GetAllRecipesAsync(
        int pageNumber,
        int pageSize,
        RecipeCategory? category = null,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = (await _unitOfWork.Recipes.GetAllAsync(cancellationToken)).AsQueryable();

        // Filter by category if specified
        if (category.HasValue)
        {
            query = query.Where(r => r.Category == category.Value);
        }

        var totalCount = query.Count();
        var recipes = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var recipeDtos = _mapper.Map<List<RecipeDto>>(recipes);

        // Check if user has favorited each recipe
        if (userId.HasValue)
        {
            var favoriteRecipeIds = (await _unitOfWork.Favorites
                .FindAsync(f => f.UserId == userId.Value, cancellationToken))
                .Select(f => f.RecipeId)
                .ToHashSet();

            foreach (var dto in recipeDtos)
            {
                dto.IsFavorited = favoriteRecipeIds.Contains(dto.Id);
            }
        }

        return new PagedResultDto<RecipeDto>
        {
            Items = recipeDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<RecipeDto> GetRecipeByIdAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var recipe = await _unitOfWork.Recipes.GetByIdAsync(id, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException(nameof(Recipe), id);
        }

        var recipeDto = _mapper.Map<RecipeDto>(recipe);

        // Check if user has favorited this recipe
        if (userId.HasValue)
        {
            recipeDto.IsFavorited = await _unitOfWork.Favorites
                .AnyAsync(f => f.UserId == userId.Value && f.RecipeId == id, cancellationToken);
        }

        return recipeDto;
    }

    public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto dto, Guid userId, CancellationToken cancellationToken = default)
    {
        var recipe = _mapper.Map<Recipe>(dto);
        recipe.CreatedByUserId = userId;
        recipe.IsFromExternalApi = false;

        await _unitOfWork.Recipes.AddAsync(recipe, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RecipeDto>(recipe);
    }

    public async Task<RecipeDto> UpdateRecipeAsync(UpdateRecipeDto dto, Guid userId, CancellationToken cancellationToken = default)
    {
        var recipe = await _unitOfWork.Recipes.GetByIdAsync(dto.Id, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException(nameof(Recipe), dto.Id);
        }

        // Only the creator can update their own recipe
        if (recipe.CreatedByUserId != userId)
        {
            throw new UnauthorizedAccessException("You can only update your own recipes.");
        }

        // Can't update external API recipes
        if (recipe.IsFromExternalApi)
        {
            throw new ValidationException("IsFromExternalApi", "Cannot update recipes from external API.");
        }

        _mapper.Map(dto, recipe);
        await _unitOfWork.Recipes.UpdateAsync(recipe, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RecipeDto>(recipe);
    }

    public async Task DeleteRecipeAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var recipe = await _unitOfWork.Recipes.GetByIdAsync(id, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException(nameof(Recipe), id);
        }

        // Only the creator can delete their own recipe
        if (recipe.CreatedByUserId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete your own recipes.");
        }

        // Can't delete external API recipes
        if (recipe.IsFromExternalApi)
        {
            throw new ValidationException("IsFromExternalApi", "Cannot delete recipes from external API.");
        }

        await _unitOfWork.Recipes.DeleteAsync(recipe, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResultDto<RecipeDto>> GetFavoriteRecipesAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var favorites = await _unitOfWork.Favorites
            .FindAsync(f => f.UserId == userId, cancellationToken);

        var recipeIds = favorites.Select(f => f.RecipeId).ToList();
        var recipes = (await _unitOfWork.Recipes.GetAllAsync(cancellationToken))
            .Where(r => recipeIds.Contains(r.Id))
            .ToList();

        var totalCount = recipes.Count;
        var pagedRecipes = recipes
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var recipeDtos = _mapper.Map<List<RecipeDto>>(pagedRecipes);

        // All are favorited
        foreach (var dto in recipeDtos)
        {
            dto.IsFavorited = true;
        }

        return new PagedResultDto<RecipeDto>
        {
            Items = recipeDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddToFavoritesAsync(Guid recipeId, Guid userId, CancellationToken cancellationToken = default)
    {
        // Check if recipe exists
        var recipe = await _unitOfWork.Recipes.GetByIdAsync(recipeId, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException(nameof(Recipe), recipeId);
        }

        // Check if already favorited
        var existingFavorite = await _unitOfWork.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId, cancellationToken);

        if (existingFavorite != null)
        {
            throw new ValidationException("RecipeId", "Recipe is already in favorites.");
        }

        var favorite = new Favorite
        {
            UserId = userId,
            RecipeId = recipeId,
            AddedAt = DateTime.UtcNow
        };

        await _unitOfWork.Favorites.AddAsync(favorite, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveFromFavoritesAsync(Guid recipeId, Guid userId, CancellationToken cancellationToken = default)
    {
        var favorite = await _unitOfWork.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId, cancellationToken);

        if (favorite == null)
        {
            throw new EntityNotFoundException("Favorite", $"UserId: {userId}, RecipeId: {recipeId}");
        }

        await _unitOfWork.Favorites.DeleteAsync(favorite, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task SyncExternalRecipesAsync(CancellationToken cancellationToken = default)
    {
        var externalRecipes = await _externalApiService.FetchRecipesAsync(50, cancellationToken);

        foreach (var recipeDto in externalRecipes)
        {
            // Check if recipe already exists
            var existingRecipe = await _unitOfWork.Recipes
                .FirstOrDefaultAsync(r => r.ExternalApiId == int.Parse(recipeDto.Id.ToString()), cancellationToken);

            if (existingRecipe == null)
            {
                var recipe = _mapper.Map<Recipe>(recipeDto);
                recipe.IsFromExternalApi = true;
                recipe.ExternalApiId = int.Parse(recipeDto.Id.ToString());
                recipe.CreatedByUserId = null;

                await _unitOfWork.Recipes.AddAsync(recipe, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

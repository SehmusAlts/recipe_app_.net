using AutoMapper;
using RecipeApp.Application.DTOs.Rating;
using RecipeApp.Application.Interfaces;
using RecipeApp.Domain.Entities;
using RecipeApp.Domain.Exceptions;
using RecipeApp.Domain.Interfaces;

namespace RecipeApp.Application.Services;

/// <summary>
/// Rating service implementation.
/// Follows Single Responsibility Principle - handles only rating business logic.
/// </summary>
public class RatingService : IRatingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RatingDto> CreateOrUpdateRatingAsync(
        CreateRatingDto dto,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // Check if recipe exists
        var recipe = await _unitOfWork.Recipes.GetByIdAsync(dto.RecipeId, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException(nameof(Recipe), dto.RecipeId);
        }

        // Check if user already rated this recipe
        var existingRating = await _unitOfWork.Ratings
            .FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == dto.RecipeId, cancellationToken);

        Rating rating;
        if (existingRating != null)
        {
            // Update existing rating
            existingRating.Value = dto.Value;
            existingRating.Comment = dto.Comment;
            existingRating.RatedAt = DateTime.UtcNow;

            await _unitOfWork.Ratings.UpdateAsync(existingRating, cancellationToken);
            rating = existingRating;
        }
        else
        {
            // Create new rating
            rating = _mapper.Map<Rating>(dto);
            rating.UserId = userId;
            rating.RatedAt = DateTime.UtcNow;

            await _unitOfWork.Ratings.AddAsync(rating, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload to get user information
        var reloadedRating = await _unitOfWork.Ratings.GetByIdAsync(rating.Id, cancellationToken);
        return _mapper.Map<RatingDto>(reloadedRating ?? rating);
    }

    public async Task<List<RatingDto>> GetRecipeRatingsAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        // Check if recipe exists
        var recipe = await _unitOfWork.Recipes.GetByIdAsync(recipeId, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException(nameof(Recipe), recipeId);
        }

        var ratings = await _unitOfWork.Ratings
            .FindAsync(r => r.RecipeId == recipeId, cancellationToken);

        return _mapper.Map<List<RatingDto>>(ratings);
    }

    public async Task<RatingDto?> GetUserRatingForRecipeAsync(
        Guid recipeId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var rating = await _unitOfWork.Ratings
            .FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId, cancellationToken);

        return rating != null ? _mapper.Map<RatingDto>(rating) : null;
    }

    public async Task DeleteRatingAsync(Guid recipeId, Guid userId, CancellationToken cancellationToken = default)
    {
        var rating = await _unitOfWork.Ratings
            .FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId, cancellationToken);

        if (rating == null)
        {
            throw new EntityNotFoundException("Rating", $"UserId: {userId}, RecipeId: {recipeId}");
        }

        await _unitOfWork.Ratings.DeleteAsync(rating, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

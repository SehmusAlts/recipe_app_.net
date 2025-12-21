using FluentValidation;
using RecipeApp.Application.DTOs.Rating;

namespace RecipeApp.Application.Validators;

/// <summary>
/// Validator for rating creation.
/// </summary>
public class CreateRatingValidator : AbstractValidator<CreateRatingDto>
{
    public CreateRatingValidator()
    {
        RuleFor(x => x.RecipeId)
            .NotEmpty().WithMessage("Recipe ID is required.");

        RuleFor(x => x.Value)
            .IsInEnum().WithMessage("Rating value must be between 1 and 5.");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comment));
    }
}

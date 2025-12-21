using FluentValidation;
using RecipeApp.Application.DTOs.Recipe;

namespace RecipeApp.Application.Validators;

/// <summary>
/// Validator for recipe creation.
/// </summary>
public class CreateRecipeValidator : AbstractValidator<CreateRecipeDto>
{
    public CreateRecipeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Recipe name is required.")
            .MaximumLength(200).WithMessage("Recipe name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Ingredients)
            .NotEmpty().WithMessage("At least one ingredient is required.")
            .Must(x => x.Count > 0).WithMessage("At least one ingredient is required.");

        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("Instructions are required.")
            .MaximumLength(4000).WithMessage("Instructions must not exceed 4000 characters.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid recipe category.");

        RuleFor(x => x.PreparationTimeMinutes)
            .GreaterThan(0).WithMessage("Preparation time must be greater than 0.");

        RuleFor(x => x.CookingTimeMinutes)
            .GreaterThan(0).WithMessage("Cooking time must be greater than 0.");

        RuleFor(x => x.Servings)
            .GreaterThan(0).WithMessage("Servings must be greater than 0.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }
}

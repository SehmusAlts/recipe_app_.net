using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities;
using RecipeApp.Domain.Enums;

namespace RecipeApp.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for Recipe entity.
/// Follows Single Responsibility Principle.
/// </summary>
public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.Ingredients)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(r => r.Instructions)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(r => r.Category)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.PreparationTimeMinutes)
            .IsRequired();

        builder.Property(r => r.CookingTimeMinutes)
            .IsRequired();

        builder.Property(r => r.Servings)
            .IsRequired();

        builder.Property(r => r.ImageUrl)
            .HasMaxLength(500);

        builder.Property(r => r.ExternalApiId)
            .IsRequired(false);

        builder.Property(r => r.IsFromExternalApi)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.CreatedByUserId)
            .IsRequired(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired(false);

        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(r => r.Name);
        builder.HasIndex(r => r.Category);
        builder.HasIndex(r => r.ExternalApiId);
        builder.HasIndex(r => r.IsFromExternalApi);

        // Relationships
        builder.HasMany(r => r.FavoritedBy)
            .WithOne(f => f.Recipe)
            .HasForeignKey(f => f.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Ratings)
            .WithOne(rat => rat.Recipe)
            .HasForeignKey(rat => rat.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore computed properties
        builder.Ignore(r => r.AverageRating);
        builder.Ignore(r => r.RatingsCount);
    }
}

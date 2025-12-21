using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for Favorite entity.
/// Follows Single Responsibility Principle.
/// </summary>
public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("Favorites");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.UserId)
            .IsRequired();

        builder.Property(f => f.RecipeId)
            .IsRequired();

        builder.Property(f => f.AddedAt)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.Property(f => f.UpdatedAt)
            .IsRequired(false);

        builder.Property(f => f.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Unique constraint: one user can favorite a recipe only once
        builder.HasIndex(f => new { f.UserId, f.RecipeId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0"); // Only for non-deleted records

        // Indexes
        builder.HasIndex(f => f.UserId);
        builder.HasIndex(f => f.RecipeId);
    }
}

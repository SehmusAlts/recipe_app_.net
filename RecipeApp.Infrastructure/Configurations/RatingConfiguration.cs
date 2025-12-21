using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for Rating entity.
/// Follows Single Responsibility Principle.
/// </summary>
public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.RecipeId)
            .IsRequired();

        builder.Property(r => r.Value)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.Comment)
            .HasMaxLength(500);

        builder.Property(r => r.RatedAt)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired(false);

        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Unique constraint: one user can rate a recipe only once
        builder.HasIndex(r => new { r.UserId, r.RecipeId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0"); // Only for non-deleted records

        // Indexes
        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => r.RecipeId);
        builder.HasIndex(r => r.Value);
    }
}

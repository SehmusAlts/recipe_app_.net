using RecipeApp.Domain.Entities;

namespace RecipeApp.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern interface.
/// Provides access to repositories and manages transactions.
/// Follows Dependency Inversion Principle.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Users repository
    /// </summary>
    IRepository<User> Users { get; }

    /// <summary>
    /// Recipes repository
    /// </summary>
    IRepository<Recipe> Recipes { get; }

    /// <summary>
    /// Favorites repository
    /// </summary>
    IRepository<Favorite> Favorites { get; }

    /// <summary>
    /// Ratings repository
    /// </summary>
    IRepository<Rating> Ratings { get; }

    /// <summary>
    /// Saves all changes to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a database transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

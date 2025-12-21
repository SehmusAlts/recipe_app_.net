using Microsoft.EntityFrameworkCore.Storage;
using RecipeApp.Domain.Entities;
using RecipeApp.Domain.Interfaces;
using RecipeApp.Infrastructure.Data;

namespace RecipeApp.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation.
/// Manages transactions and coordinates repositories.
/// Follows Unit of Work Pattern and Single Responsibility Principle.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<User>? _users;
    private IRepository<Recipe>? _recipes;
    private IRepository<Favorite>? _favorites;
    private IRepository<Rating>? _ratings;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IRepository<User> Users
    {
        get { return _users ??= new Repository<User>(_context); }
    }

    public IRepository<Recipe> Recipes
    {
        get { return _recipes ??= new Repository<Recipe>(_context); }
    }

    public IRepository<Favorite> Favorites
    {
        get { return _favorites ??= new Repository<Favorite>(_context); }
    }

    public IRepository<Rating> Ratings
    {
        get { return _ratings ??= new Repository<Rating>(_context); }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

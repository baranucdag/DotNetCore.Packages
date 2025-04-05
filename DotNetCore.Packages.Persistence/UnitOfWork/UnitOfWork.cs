using DotNetCore.Packages.Domain.UnitOfWork;
using DotNetCore.Packages.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotNetCore.Packages.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly EFBaseDbContext _context;
    private IDbContextTransaction _currentTransaction;

    public UnitOfWork(EFBaseDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null,
        Action<Exception> exceptionAction = null)
    {
        TResult result;

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                result = action();
                _context.SaveChanges();
                transaction.Commit();

                successAction?.Invoke();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                exceptionAction?.Invoke(ex);
                throw;
            }
        }

        return result;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
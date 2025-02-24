using FluentResults;
using IdentityService.Domain.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IdentityService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail(new Error("Failed to save changes").CausedBy(ex));
        }
    }

    public async Task<Result<T>> ExecuteTransactionAsync<T>(Func<Task<Result<T>>> operation)
    {
        var isRootTransaction = _currentTransaction == null;

        if (isRootTransaction)
            _currentTransaction = await _context.Database.BeginTransactionAsync();

        async Task<Result<T>> RollbackAndReturn(Result<T> result)
        {
            if (_currentTransaction is not null)
                await _currentTransaction.RollbackAsync();

            return result;
        }

        try
        {
            var result = await operation();
            if (result.IsFailed)
                return await RollbackAndReturn(result);

            if (isRootTransaction)
            {
                var saveResult = await SaveChangesAsync();
                if (saveResult.IsFailed)
                    return await RollbackAndReturn(saveResult);

                if (_currentTransaction is not null)
                    await _currentTransaction.CommitAsync();

                _currentTransaction = null;
            }

            return result;
        }
        catch
        {
            return await RollbackAndReturn(Result.Fail(new Error("Failed to execute transaction")));
        }
    }
}

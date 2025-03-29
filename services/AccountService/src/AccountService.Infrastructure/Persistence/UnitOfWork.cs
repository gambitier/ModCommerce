using FluentResults;
using AccountService.Domain.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AccountService.Domain.Interfaces.Events;

namespace AccountService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserServiceDbContext _context;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(UserServiceDbContext context, IDomainEventPublisher domainEventPublisher)
    {
        _context = context;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Publishes domain events for entities that were modified during SaveChanges.
    /// Only publishes if there is no active transaction to prevent premature event publishing.
    /// Domain events should only be published after the entire transaction is committed
    /// to maintain data consistency and prevent side effects from failed transactions.
    /// </summary>
    /// <remarks>
    /// If domain events were published during an active transaction and that transaction
    /// later rolled back, the events would represent state changes that never actually
    /// persisted to the database.
    /// </remarks>
    private async Task PublishDomainEvents()
    {
        if (_currentTransaction is not null)
            return;

        var domainEvents = _context.ChangeTracker.Entries<IHasDomainEvents>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _domainEventPublisher.PublishAsync(domainEvent);
        }

        _context.ChangeTracker.Entries<IHasDomainEvents>()
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());
    }

    public async Task<Result> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();

            // If PublishDomainEvents is called within a transaction, events won't be published due to guard clause
            // If PublishDomainEvents is called outside transaction, events will be published immediately
            await PublishDomainEvents();

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
            {
                await _currentTransaction.RollbackAsync();
                _currentTransaction = null;
            }

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
                await PublishDomainEvents();
            }

            return result;
        }
        catch
        {
            return await RollbackAndReturn(Result.Fail(new Error("Failed to execute transaction")));
        }
    }
}

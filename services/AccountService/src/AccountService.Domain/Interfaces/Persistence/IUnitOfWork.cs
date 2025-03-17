using FluentResults;

namespace AccountService.Domain.Interfaces.Persistence;

/// <summary>
/// Provides transaction and persistence management for the application.
/// </summary>
/// <remarks>
/// Transaction Behaviors:
/// 
/// 1. Implicit Transactions (Using SaveChangesAsync):
///    - EF Core automatically wraps SaveChanges/SaveChangesAsync in a transaction
///    - Suitable for single operation or simple CRUD
///    - Example:
///      await _repository.CreateAsync(entity);
///      await _unitOfWork.SaveChangesAsync();
/// 
/// 2. Explicit Transactions (Using ExecuteTransactionAsync):
///    - Manually controls transaction boundaries
///    - Required when:
///      a) Multiple operations must succeed or fail together
///      b) Working with multiple databases
///      c) Need transaction isolation level control
///      d) Integrating with external services where rollback might be needed
///    - Example:
///      await _unitOfWork.ExecuteTransactionAsync(async () => {
///          await _repo1.DeleteAsync(id);
///          await _repo2.CreateAsync(newEntity);
///          return Result.Ok();
///      });
/// 
/// When to Use Each:
/// 
/// Use Implicit (SaveChangesAsync):
/// - Single table/entity operations
/// - Simple CRUD operations
/// - When automatic transaction behavior is sufficient
/// - Performance is a priority (less overhead)
/// 
/// Use Explicit (ExecuteTransactionAsync):
/// - Multiple related operations that must be atomic
/// - Complex business operations
/// - When you need control over transaction scope
/// - Distributed systems or external service integration
/// </remarks>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all changes made in this context to the database.
    /// This method is wrapped in an implicit transaction by EF Core.
    /// </summary>
    /// <returns>A Result indicating success or failure.</returns>
    Task<Result> SaveChangesAsync();

    /// <summary>
    /// Executes an operation within an explicit transaction.
    /// The transaction will automatically rollback if the operation fails.
    /// </summary>
    /// <param name="operation">The operation to execute within the transaction.</param>
    /// <returns>The result of the operation.</returns>
    Task<Result<T>> ExecuteTransactionAsync<T>(Func<Task<Result<T>>> operation);
}

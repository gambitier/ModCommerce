using FluentResults;

namespace IdentityService.Domain.Errors;

/// <summary>
/// Base class for business/domain validation errors
/// This class is abstract to allow for future extension but no direct instantiation
/// For this class errors we will return 400 Bad Request status code
/// </summary>
public abstract class DomainError(string errorCode, string message) : Error(message)
{
    public string ErrorCode { get; } = errorCode;
}

// Domain-specific validation errors
public sealed class ValidationError(string errorCode, string message) : DomainError(errorCode, message);

// ====================================================================
// Specific error types that don't inherit from DomainError
// classes that inherit from Error are not domain errors
// and they needs to be handled differently, different status codes etc
// ====================================================================

public sealed class UnauthorizedError(string errorCode, string message) : Error(message)
{
    public string ErrorCode { get; } = errorCode;
}

public sealed class NotFoundError(string errorCode, string message) : Error(message)
{
    public string ErrorCode { get; } = errorCode;
}

public sealed class ConflictError(string errorCode, string message) : Error(message)
{
    public string ErrorCode { get; } = errorCode;
}

public sealed class InternalError(string errorCode, string message) : Error(message)
{
    public string ErrorCode { get; } = errorCode;
}

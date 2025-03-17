using FluentResults;
using FluentResults.Extensions.AspNetCore;
using IdentityService.Domain.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace IdentityService.API.ErrorHandling;

/// <summary>
/// Custom result profile for the IdentityService.API that transforms FluentResults errors into standard HTTP responses.
/// Without this profile, the API would not properly translate FluentResults errors into RFC 7807 problem details format
/// that clients expect from REST APIs.
/// </summary>
public class CustomAspNetCoreResultEndpointProfile : DefaultAspNetCoreResultEndpointProfile
{
    /// <summary>
    /// This is used to create <see cref="ProblemDetails" /> for the error
    /// Without this server wont return the error in the standard RFC 7807 problem details format
    /// </summary>
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomAspNetCoreResultEndpointProfile> _logger;

    public CustomAspNetCoreResultEndpointProfile(
        ProblemDetailsFactory problemDetailsFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CustomAspNetCoreResultEndpointProfile> logger)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public override ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
    {
        var result = context.Result;
        var error = result.Errors.First();
        var statusCode = GetStatusCode(error);

        if (_httpContextAccessor.HttpContext == null)
        {
            _logger.LogWarning("HttpContext was null when handling error: {ErrorMessage}. Creating new default context.", error.Message);
        }

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            _httpContextAccessor.HttpContext ?? new DefaultHttpContext(),
            statusCode: statusCode,
            detail: error.Message);

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCode(IError error) => error switch
    {
        UnauthorizedError => StatusCodes.Status401Unauthorized,
        NotFoundError => StatusCodes.Status404NotFound,
        ConflictError => StatusCodes.Status409Conflict,
        ValidationError => StatusCodes.Status400BadRequest,
        InternalError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };
}
using System.Threading.RateLimiting;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddApiRateLimiting(
        this IServiceCollection services,
        RateLimitOptions options)
    {
        services.AddRateLimiter(config =>
        {
            // Default policy for all endpoints
            config.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 100,         // Default limit
                        QueueLimit = 0,           // No queue
                        ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                        TokensPerPeriod = 100,
                        AutoReplenishment = true
                    }));

            // Specific policy for token endpoint
            config.AddPolicy("token", context =>
                RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = options.TokenEndpoint.PermitLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = options.TokenEndpoint.QueueLimit,
                        ReplenishmentPeriod = TimeSpan.Parse(options.TokenEndpoint.Window),
                        TokensPerPeriod = 1,
                        AutoReplenishment = true
                    }));
        });

        return services;
    }

    private static string GetPartitionKey(HttpContext context)
    {
        // Get the real IP, considering forwarded headers
        var clientIp = context.Connection.RemoteIpAddress?.ToString()
            ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
            ?? context.Request.Headers.Host.ToString();

        // For unauthenticated requests, use IP
        if (context.User?.Identity?.IsAuthenticated != true)
            return $"ip:{clientIp}";

        // For authenticated requests, use userId
        return $"user:{context.User.Identity.Name}";
    }
}
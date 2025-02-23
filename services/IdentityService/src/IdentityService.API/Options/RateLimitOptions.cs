using System.ComponentModel.DataAnnotations;

public class RateLimitOptions
{
    public GlobalOptions Global { get; set; } = new();
    public TokenEndpointOptions TokenEndpoint { get; set; } = new();
}

public class GlobalOptions
{
    public int PermitLimit { get; set; } = 100;
    public string Window { get; set; } = "1m";
    public int TokensPerPeriod { get; set; } = 100;
}

public class TokenEndpointOptions
{
    [Range(1, 100)]
    public int PermitLimit { get; set; }

    [Required]
    [RegularExpression(@"^\d+[smhd]$", ErrorMessage = "Window must be in format: number + s/m/h/d")]
    public string Window { get; set; } = string.Empty;

    [Range(0, 10)]
    public int QueueLimit { get; set; }
}

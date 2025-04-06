using System.Security.Cryptography;
using System.Text;

namespace AccountService.Domain.Utils;

public static class GuidUtils
{
    public static string NewGuid() => Guid.NewGuid().ToString();

    /// <summary>
    /// Create a deterministic GUID from a string.
    /// </summary>
    /// <param name="input">The input string to create the GUID from.</param>
    /// <returns>A deterministic GUID.</returns>
    public static Guid CreateDeterministicGuid(string input)
    {
        byte[] hash = MD5.HashData(Encoding.Default.GetBytes(input));
        return new Guid(hash);
    }
}

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace IdentityService.Domain.Constants;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TokenType
{
    [EnumMember(Value = "Bearer")]
    Bearer,
    [EnumMember(Value = "MAC")]
    Mac
}

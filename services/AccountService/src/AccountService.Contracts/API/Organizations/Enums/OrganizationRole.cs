using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AccountService.Contracts.API.Organizations.Enums
{
    /// <summary>
    /// Defines the possible roles a user can have within an organization
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))] // Forces string serialization
    public enum OrganizationRole
    {
        /// <summary>
        /// Full control over the organization, including managing other members
        /// </summary>
        [EnumMember(Value = "Owner")]
        Owner = 0,

        /// <summary>
        /// Administrative privileges within the organization
        /// </summary>
        [EnumMember(Value = "Admin")]
        Admin = 1,

        /// <summary>
        /// Standard member access to the organization
        /// </summary>
        [EnumMember(Value = "Member")]
        Member = 2
    }
}

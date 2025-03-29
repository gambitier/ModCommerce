using AccountService.Contracts.API.Organizations.Enums;
using System.ComponentModel.DataAnnotations;
namespace AccountService.Contracts.API.Organizations.Requests;

/// <summary>
/// A request for inviting a user to an organization.
/// </summary>
public class InviteOrganizationMemberRequest
{
    /// <summary>
    /// The id of the user to invite to the organization.
    /// </summary>
    [Required(ErrorMessage = "User ID is required")]
    public required string UserId { get; init; }

    /// <summary>
    /// The role of the user in the organization.
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    [EnumDataType(typeof(OrganizationRole), ErrorMessage = "Invalid role")]
    public required OrganizationRole Role { get; init; }
}
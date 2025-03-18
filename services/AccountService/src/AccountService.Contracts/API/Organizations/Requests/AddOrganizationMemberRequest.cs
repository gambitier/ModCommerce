using AccountService.Contracts.API.Organizations.Enums;
using System.ComponentModel.DataAnnotations;
namespace AccountService.Contracts.API.Organizations.Requests;

public class AddOrganizationMemberRequest
{
    [Required(ErrorMessage = "User ID is required")]
    public required string UserId { get; init; }

    [Required(ErrorMessage = "Role is required")]
    [EnumDataType(typeof(OrganizationRole), ErrorMessage = "Invalid role")]
    public required OrganizationRole Role { get; init; }
}
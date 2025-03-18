using System.ComponentModel.DataAnnotations;

namespace AccountService.Domain.Models.Organizations.Enums;

public enum UserOrganizationMembershipRole
{
    [Display(Name = "Owner")]
    Owner,

    [Display(Name = "Admin")]
    Admin,

    [Display(Name = "Member")]
    Member,
}
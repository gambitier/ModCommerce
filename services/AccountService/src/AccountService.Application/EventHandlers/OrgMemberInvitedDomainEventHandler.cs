using AccountService.Domain.Events.OrgMember;
using AccountService.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentResults;

namespace AccountService.Application.EventHandlers;

public class OrgMemberInvitedDomainEventHandlerError
{
    public static Error UserProfileNotFound(string userId) =>
        new Error($"User profile not found for userId: {userId}");

    public static Error OrganizationNotFound(Guid organizationId) =>
        new Error($"Organization not found for organizationId: {organizationId}");
}

public class OrgMemberInvitedDomainEventHandler : INotificationHandler<OrgMemberInvitedDomainEvent>
{
    private readonly ILogger<OrgMemberInvitedDomainEventHandler> _logger;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public OrgMemberInvitedDomainEventHandler(
        ILogger<OrgMemberInvitedDomainEventHandler> logger,
        IUserProfileRepository userProfileRepository,
        IOrganizationRepository organizationRepository)
    {
        _logger = logger;
        _userProfileRepository = userProfileRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task Handle(
        OrgMemberInvitedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await HandleNotification(notification);
            if (result.IsFailed)
            {
                var errors = string.Join("\n", result.Errors.Select(e => e.Message));
                _logger.LogError("Failed to handle OrganMemberInvitedDomainEvent: {Errors}", errors);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling the OrganMemberInvitedDomainEvent");
        }
    }


    private async Task<Result> HandleNotification(OrgMemberInvitedDomainEvent notification)
    {
        var userProfiles = await _userProfileRepository.GetUserProfilesAsync([
            notification.UserId,
            notification.InvitedByUserId
        ]);

        var userProfile = userProfiles[notification.UserId];
        var invitedByUserProfile = userProfiles[notification.InvitedByUserId];

        if (userProfile == null)
            return Result.Fail(OrgMemberInvitedDomainEventHandlerError.UserProfileNotFound(notification.UserId));

        if (invitedByUserProfile == null)
            return Result.Fail(OrgMemberInvitedDomainEventHandlerError.UserProfileNotFound(notification.InvitedByUserId));

        var orgDtoResult = await _organizationRepository.GetByIdAsync(notification.OrganizationId);
        if (orgDtoResult.IsFailed)
            return Result.Fail(OrgMemberInvitedDomainEventHandlerError.OrganizationNotFound(notification.OrganizationId));

        var orgDto = orgDtoResult.Value;

        var emailContentText = $@"
You are invited by {invitedByUserProfile.FirstName} {invitedByUserProfile.LastName} to join {orgDto.Name}.

Use the code below to join the organization:
{notification.InvitationId}

This invitation will expire on {notification.ExpiresAt}.

Looking forward to seeing you in {orgDto.Name}!
        ";

        // TODO: send invitation email
        _logger.LogInformation("Sending invitation email to {Email}", userProfile.Email);
        _logger.LogInformation("Email content: {EmailContent}", emailContentText);
        return Result.Ok();
    }
}
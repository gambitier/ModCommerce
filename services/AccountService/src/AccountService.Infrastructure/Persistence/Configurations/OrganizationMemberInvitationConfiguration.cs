using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AccountService.Infrastructure.Persistence.Entities;

namespace AccountService.Infrastructure.Persistence.Configurations;

public class OrganizationMemberInvitationConfiguration : IEntityTypeConfiguration<Entities.OrganizationMemberInvitation>
{
    public void Configure(EntityTypeBuilder<Entities.OrganizationMemberInvitation> builder)
    {
        builder.ToTable("OrganizationMemberInvitations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.OrganizationId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(x => x.Role).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        // userid, orgId and role is composite key
        builder.HasKey(x => new { x.UserId, x.OrganizationId, x.Role });
        builder.HasIndex(x => new { x.UserId, x.OrganizationId, x.Role }).IsUnique();

        // userId is foreign key to UserProfile table's userId
        builder.HasOne<UserProfileEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // organizationId is foreign key to Organization table's id
        builder.HasOne<OrganizationEntity>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);


        // Order of the columns
        builder.Property(x => x.Id).HasColumnOrder(1);
        builder.Property(x => x.UserId).HasColumnOrder(2);
        builder.Property(x => x.InvitedByUserId).HasColumnOrder(3);
        builder.Property(x => x.OrganizationId).HasColumnOrder(4);
        builder.Property(x => x.AcceptedAt).HasColumnOrder(5);
        builder.Property(x => x.RejectedAt).HasColumnOrder(6);
        builder.Property(x => x.CreatedAt).HasColumnOrder(7);
        builder.Property(x => x.UpdatedAt).HasColumnOrder(8);
        builder.Property(x => x.ExpiresAt).HasColumnOrder(9);
    }
}
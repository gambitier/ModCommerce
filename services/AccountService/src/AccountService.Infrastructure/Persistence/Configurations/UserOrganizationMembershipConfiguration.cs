using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AccountService.Infrastructure.Persistence.Entities;

namespace AccountService.Infrastructure.Persistence.Configurations;

public class UserOrganizationMembershipConfiguration : IEntityTypeConfiguration<UserOrganizationMembership>
{
    public void Configure(EntityTypeBuilder<UserOrganizationMembership> builder)
    {
        builder.ToTable("UserOrganizationMemberships");

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
    }
}
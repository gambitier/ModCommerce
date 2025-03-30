using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AccountService.Infrastructure.Persistence.Entities;

namespace AccountService.Infrastructure.Persistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.ToTable("UserProfiles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId).IsUnique();

        // Order of the columns
        builder.Property(x => x.Id).HasColumnOrder(1);
        builder.Property(x => x.UserId).HasColumnOrder(2);
        builder.Property(x => x.Username).HasColumnOrder(3);
        builder.Property(x => x.Email).HasColumnOrder(4);
        builder.Property(x => x.Status).HasColumnOrder(5);
        builder.Property(x => x.FirstName).HasColumnOrder(6);
        builder.Property(x => x.LastName).HasColumnOrder(7);
        builder.Property(x => x.CreatedAt).HasColumnOrder(8);
        builder.Property(x => x.UpdatedAt).HasColumnOrder(9);
    }
}
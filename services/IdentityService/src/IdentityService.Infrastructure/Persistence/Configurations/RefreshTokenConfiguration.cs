using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityService.Infrastructure.Persistence.Entities;

namespace IdentityService.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id)
            .ValueGeneratedOnAdd(); // EF Core will handle GUID generation

        builder.Property(rt => rt.Token).IsRequired();
        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.Property(rt => rt.UserId).IsRequired();
        builder.HasOne<IdentityUser>()          // RefreshToken has one User
            .WithMany()                         // User can have many RefreshTokens
            .HasForeignKey(rt => rt.UserId)     // Using UserId as the foreign key
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        builder.Property(rt => rt.ExpiresAt).IsRequired();
        builder.Property(rt => rt.CreatedAt).IsRequired();
        builder.Property(rt => rt.IsRevoked).IsRequired();


        // Order of the columns
        builder.Property(x => x.Id).HasColumnOrder(1);
        builder.Property(x => x.UserId).HasColumnOrder(2);
        builder.Property(x => x.Token).HasColumnOrder(3);
        builder.Property(x => x.CreatedAt).HasColumnOrder(4);
        builder.Property(x => x.ExpiresAt).HasColumnOrder(5);
        builder.Property(x => x.IsRevoked).HasColumnOrder(6);
    }
}
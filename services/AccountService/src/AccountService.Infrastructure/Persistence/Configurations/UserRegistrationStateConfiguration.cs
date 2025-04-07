using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AccountService.Infrastructure.Sagas;

namespace AccountService.Infrastructure.Persistence.Configurations;

public class UserRegistrationStateConfiguration : IEntityTypeConfiguration<UserRegistrationState>
{
    public void Configure(EntityTypeBuilder<UserRegistrationState> builder)
    {
        builder.ToTable("UserRegistrationStates");

        builder.HasKey(x => x.CorrelationId);
        builder.Property(x => x.CorrelationId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId).IsUnique();

        // Order of the columns
        builder.Property(x => x.CorrelationId).HasColumnOrder(1);
        builder.Property(x => x.CurrentState).HasColumnOrder(2);
        builder.Property(x => x.UserId).HasColumnOrder(3);
        builder.Property(x => x.Email).HasColumnOrder(4);
        builder.Property(x => x.Username).HasColumnOrder(5);
        builder.Property(x => x.CreatedAt).HasColumnOrder(6);
        builder.Property(x => x.EmailConfirmedAt).HasColumnOrder(7);
        builder.Property(x => x.IsProfileCreated).HasColumnOrder(8);
    }
}
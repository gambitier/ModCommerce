using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AccountService.Infrastructure.Persistence.Entities;

namespace AccountService.Infrastructure.Persistence.Configurations;

public class UserRegistrationSagaStateConfiguration : IEntityTypeConfiguration<UserRegistrationSagaState>
{
    public void Configure(EntityTypeBuilder<UserRegistrationSagaState> builder)
    {
        builder.ToTable("UserRegistrationSagaStates");

        builder.HasKey(x => x.CorrelationId);

        builder.Property(x => x.UserId).IsRequired();
        builder.HasIndex(x => x.UserId).IsUnique();

        // Order of the columns
        builder.Property(x => x.CorrelationId).HasColumnOrder(1);
        builder.Property(x => x.UserId).HasColumnOrder(2);
        builder.Property(x => x.CurrentState).HasColumnOrder(3);
        builder.Property(x => x.Email).HasColumnOrder(4);
        builder.Property(x => x.IsProfileCreated).HasColumnOrder(5);
        builder.Property(x => x.IsEmailConfirmed).HasColumnOrder(6);
        builder.Property(x => x.UpdatedAt).HasColumnOrder(7);
    }
}
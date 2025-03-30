using AccountService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<OrganizationEntity>
{
    public void Configure(EntityTypeBuilder<OrganizationEntity> builder)
    {
        builder.ToTable("Organizations");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Description);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        // Order of the columns
        builder.Property(e => e.Id).HasColumnOrder(1);
        builder.Property(e => e.Name).HasColumnOrder(2);
        builder.Property(e => e.Description).HasColumnOrder(3);
        builder.Property(e => e.CreatedAt).HasColumnOrder(4);
        builder.Property(e => e.UpdatedAt).HasColumnOrder(5);
    }
}
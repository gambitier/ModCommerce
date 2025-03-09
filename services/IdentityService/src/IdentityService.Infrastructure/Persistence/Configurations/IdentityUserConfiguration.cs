using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityService.Infrastructure.Persistence.Entities;

namespace IdentityService.Infrastructure.Persistence.Configurations;

public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}

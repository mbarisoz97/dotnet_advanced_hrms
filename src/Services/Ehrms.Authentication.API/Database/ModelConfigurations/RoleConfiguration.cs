using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.Authentication.API.Database.ModelConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasMany(r => r.UserRoles)
           .WithOne(ur => ur.Role)
           .HasForeignKey(ur => ur.RoleId)
           .IsRequired();
    }
}

using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endava.BookSharing.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(x => new
        {
            x.UserId,
            x.RoleId
        });

        builder.HasOne(x => x.RoleDb)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId).IsRequired();

        builder.HasOne(x => x.UserDb)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.UserId).IsRequired();
    }
}
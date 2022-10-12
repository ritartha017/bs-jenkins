using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endava.BookSharing.Infrastructure.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<AuthorDb>
{
    public void Configure(EntityTypeBuilder<AuthorDb> builder)
    {
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(61);
    }
}
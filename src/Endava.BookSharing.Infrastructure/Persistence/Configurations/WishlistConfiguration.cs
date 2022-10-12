using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endava.BookSharing.Infrastructure.Persistence.Configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<WishlistDb>
{
    public void Configure(EntityTypeBuilder<WishlistDb> builder)
    {
        builder.HasKey(x => new
        {
            x.UserId,
            x.BookId
        });
    }
}
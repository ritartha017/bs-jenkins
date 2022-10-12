using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endava.BookSharing.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<BookDb>
{
    public void Configure(EntityTypeBuilder<BookDb> builder)
    {
        builder.HasMany(x => x.Genres)
            .WithMany(x => x.Books);

        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Book)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Reviews_Book");
    }
}

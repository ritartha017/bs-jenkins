using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endava.BookSharing.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<ReviewDb>
{
    public void Configure(EntityTypeBuilder<ReviewDb> builder)
    {
        builder.Property(x => x.Content)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Book)
            .WithMany(x => x.Reviews)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Reviews_Book");
    }
}

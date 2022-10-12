using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<UserDb, RoleDb, string,
    IdentityUserClaim<string>,
    UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options)
    {
    }

    public DbSet<AuthorDb> Authors { get; set; }
    public DbSet<FileDb> Files { get; set; }
    public DbSet<BookDb> Books { get; set; }
    public DbSet<GenreDb> Genres { get; set; }
    public DbSet<LanguageDb> Languages { get; set; }
    public DbSet<ReviewDb> Reviews { get; set; }
    public DbSet<WishlistDb> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
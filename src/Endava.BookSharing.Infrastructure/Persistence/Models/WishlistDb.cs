using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;

namespace Endava.BookSharing.Infrastructure.Persistence.Models;

public class WishlistDb
{
    public string UserId { get; set; } = null!;
    public string BookId { get; set; } = null!;

    public virtual UserDb User { get; set; } = null!;
    public virtual BookDb Book { get; set; } = null!;
}
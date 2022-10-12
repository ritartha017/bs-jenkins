using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;

namespace Endava.BookSharing.Infrastructure.Persistence.Models;

public class ReviewDb
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public int Rating { get; set; }
    public DateTime PostedAt { get; set; }

    public virtual BookDb Book { get; set; } = null!;
    public virtual UserDb PostedBy { get; set; } = null!;
    public string PostedById { get; set; } = null!;
}

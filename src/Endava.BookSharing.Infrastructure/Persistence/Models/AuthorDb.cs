using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;

namespace Endava.BookSharing.Infrastructure.Persistence.Models;

public class AuthorDb
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public bool IsApproved { get; set; }
    public virtual UserDb AddedBy { get; set; } = null!;
}
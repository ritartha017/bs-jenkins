using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;

namespace Endava.BookSharing.Infrastructure.Persistence.Models;

public class BookDb
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public bool IsDraft { get; set; }
    public DateTime PublicationDate { get; set; }

    public string? CoverId { get; set; } = null!;
    public string AuthorId { get; set; } = null!;
    public string LanguageId { get; set; } = null!;
    public string OwnerId { get; set; } = null!;

    public virtual FileDb? Cover { get; set; } = null!;
    public virtual AuthorDb Author { get; set; } = null!;
    public virtual LanguageDb Language { get; set; } = null!;
    public virtual UserDb Owner { get; set; } = null!;
    public virtual ICollection<GenreDb> Genres { get; set; } = new List<GenreDb>();
    public virtual ICollection<ReviewDb> Reviews { get; set; } = null!;
}
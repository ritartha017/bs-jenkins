namespace Endava.BookSharing.Infrastructure.Persistence.Models;

public class GenreDb
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public virtual ICollection<BookDb> Books { get; private set; } = new List<BookDb>();
}
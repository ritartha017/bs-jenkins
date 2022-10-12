namespace Endava.BookSharing.Domain.Entities;

public class Book
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string OwnerId { get; set; } = null!;
    public string AuthorId { get; set; } = null!;
    public bool IsDraft { get; set; }
    public DateTime PublicationDate { get; set; }
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public string LanguageId { get; set; } = null!;
    public string? CoverId { get; set; }
}

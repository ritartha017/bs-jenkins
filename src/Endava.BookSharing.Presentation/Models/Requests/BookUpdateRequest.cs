
namespace Endava.BookSharing.Presentation.Models.Requests;

public class BookUpdateRequest
{
    public string Title { get; set; } = null!;
    public string? AuthorName { get; set; } = null!;
    public string? AuthorId { get; set; } = null!;
    public string PublicationDate { get; set; } = null!;
    public ICollection<string> Genres { get; set; } = null!;
    public string LanguageId { get; set; } = null!;
    public IFormFile? Cover { get; set; } = null!;
}

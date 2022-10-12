using Microsoft.AspNetCore.Http;

namespace Endava.BookSharing.Application.Models.Requests;

public class CreateBookRequest
{
    public string Title { get; set; } = null!;
    public string? AuthorId { get; set; }
    public string? AuthorFullName { get; set; }
    public string PublicationDate { get; set; } = null!;
    public ICollection<string> GenreIds { get; set; } = null!;
    public IFormFile File { get; set;  } = null!;
    public string LanguageId { get; set; } = null!;
}

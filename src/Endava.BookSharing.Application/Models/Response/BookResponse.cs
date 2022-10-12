using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Models.Response;

public class BookResponse
{
    public string Cover { get; set; } = null!;
    public string Title { get; set; } = null!;
    public double Rating { get; set; }
    public UserResponse UploadedBy { get; set; } = null!;
    public AuthorResponse Author { get; set; } = null!;
    public Language Language { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public DateTime PublicationDate { get; set; }
    public bool IsEditable { get; set; }

}
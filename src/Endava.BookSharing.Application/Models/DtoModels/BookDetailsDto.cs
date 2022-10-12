using Endava.BookSharing.Application.Models.Response;
using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Models.DtoModels;

public class BookDetailsDto
{
    public string Cover { get; set; } = null!;
    public string Title { get; set; } = null!;
    public UserResponse UploadedBy { get; set; } = null!;
    public AuthorResponse Author { get; set; } = null!;
    public Language Language { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public DateTime PublicationDate { get; set; }
    public bool IsEditable { get; set; }
}
using Endava.BookSharing.Application.Models;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.UpdateBook;

public class UpdateBookCommand : IRequest<Unit>
{
    public UpdateBookCommand(string bookId, AuthenticatedUser user)
    {
        BookId = bookId;
        CurrentUser = user;
    }
    public string BookId { get; }
    public string Title { get; init; } = null!;
    public string PublicationDate { get; init; } = null!;
    public ICollection<string> GenreIds { get; init; } = null!;
    public string LanguageId { get; init; } = null!;
    public string? AuthorId { get; set; } = null!;
    public string? AuthorName { get; set; } = null!;
    public string? FileType { get; init; } = null!;
    public byte[]? RawFile { get; init; } = null!;
    public AuthenticatedUser CurrentUser { get; }
}

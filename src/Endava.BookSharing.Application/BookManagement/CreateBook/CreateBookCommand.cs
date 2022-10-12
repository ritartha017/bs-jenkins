using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Domain.Abstractions;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.CreateBook;

public class CreateBookCommand : IRequest<Unit>
{
    public CreateBookCommand(CreateBookRequest newBook, string currentUser, IFileData file)
    {
        Title = newBook.Title;
        CurrentUserId = currentUser;
        AuthorId = newBook.AuthorId!;
        AuthorFullName = newBook.AuthorFullName!;
        PublicationDate = newBook.PublicationDate;
        GenreIds = newBook.GenreIds;
        LanguageId = newBook.LanguageId;
        CoverData = file;
    }
    public string Title { get; }
    public string CurrentUserId { get; }
    public string? AuthorId { get; }
    public string? AuthorFullName { get; }
    public string PublicationDate { get; }
    public ICollection<string> GenreIds { get; }
    public string LanguageId { get; }
    public IFileData CoverData { get; }
}

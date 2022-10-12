using Endava.BookSharing.Application.BookManagement.ListBooks;
using Endava.BookSharing.Application.Filters;
using Endava.BookSharing.Application.Models.DtoModels;
using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Abstract;

public interface IBookRepository
{
    Task<ICollection<Book>> GetByOwnerIdAsync(string ownerId, PaginationFilter filter,
        CancellationToken cancellationToken);
    Task<int> GetCountByOwnerIdAsync(string ownerId, CancellationToken cancellationToken);

    Task<bool> UpdateAsync(Book newBookData, bool deleteCover, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string bookId, CancellationToken cancellationToken);

    Task<bool> CreateAsync(Book book, CancellationToken cancellationToken);

    Task<Book?> GetByIdAsync(string bookId, CancellationToken cancellationToken);

    Task<BookDetailsDto?> GetByIdDetailsAsync(string bookId, CancellationToken cancellationToken);

    Task<bool> DeleteCoverAsync(string bookId, CancellationToken cancellationToken);
    Task<ICollection<Book>> GetForPaginationAsync(FilterBookParams filters, CancellationToken cancellationToken);
}
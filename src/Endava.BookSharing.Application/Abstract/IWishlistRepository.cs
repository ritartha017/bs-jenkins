using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Abstract;

public interface IWishlistRepository
{
    Task<bool> AddBookAsync(string bookId, string userId, CancellationToken cancellationToken);

    Task<IEnumerable<Book>> GetBooksByUserIdAsync(string userId, CancellationToken cancellationToken);

    Task<bool> DeleteBookFromUserList(string bookId, string userId, CancellationToken cancellationToken);

    Task<IEnumerable<Book>> GetForPaginationBooksAsync(string userId, int skipNoOfItems, int takeNoOfItems, CancellationToken cancellationToken);
}
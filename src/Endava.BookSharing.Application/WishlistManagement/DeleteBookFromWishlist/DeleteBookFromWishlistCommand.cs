using MediatR;

namespace Endava.BookSharing.Application.WishlistManagement.DeleteBookFromWishlist;

public record DeleteBookFromWishlistCommand(string BookId, string UserId) : IRequest<Unit>;
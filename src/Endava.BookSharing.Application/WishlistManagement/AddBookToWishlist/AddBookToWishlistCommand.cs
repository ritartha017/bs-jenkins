using MediatR;

namespace Endava.BookSharing.Application.WishlistManagement.AddBookToWishlist;

public record AddBookToWishlistCommand(string BookId, string UserId) : IRequest<Unit>;

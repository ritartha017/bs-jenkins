using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using MediatR;

namespace Endava.BookSharing.Application.WishlistManagement.DeleteBookFromWishlist;

public class DeleteBookFromWishlistCommandHandler : IRequestHandler<DeleteBookFromWishlistCommand, Unit>
{
    private readonly IWishlistRepository wishlistRepository;

    public DeleteBookFromWishlistCommandHandler(IWishlistRepository wishlistRepository)
    {
        this.wishlistRepository = wishlistRepository ?? throw new ArgumentNullException(nameof(wishlistRepository));
    }

    public async Task<Unit> Handle(DeleteBookFromWishlistCommand command, CancellationToken cancellationToken)
    {
        var userWishlistBooks = await wishlistRepository.GetBooksByUserIdAsync(command.UserId, cancellationToken);
        var bookFromWishlist = userWishlistBooks!.FirstOrDefault(b => b.Id == command.BookId);

        if (bookFromWishlist is null)
        {
            throw new BookSharingNotFoundException("There is no such book in wishlist.");
        }

        if (await wishlistRepository.DeleteBookFromUserList(command.BookId, command.UserId, cancellationToken) is false)
        {
            throw new BookSharingGenericException("Failed to remove book from wishlist.");
        }

        return Unit.Value;
    }
}
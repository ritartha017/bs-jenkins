using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;
using MediatR;

namespace Endava.BookSharing.Application.WishlistManagement.AddBookToWishlist;

public class AddBookToWishlistCommandHandler : IRequestHandler<AddBookToWishlistCommand, Unit>
{
    private readonly IWishlistRepository wishlistRepository;
    private readonly IBookRepository bookRepository;

    public AddBookToWishlistCommandHandler(IWishlistRepository wishlistRepository, IBookRepository bookRepository)
    {
        this.wishlistRepository = wishlistRepository ?? throw new ArgumentNullException(nameof(wishlistRepository));
        this.bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
    }

    public async Task<Unit> Handle(AddBookToWishlistCommand command, CancellationToken cancellationToken)
    {
        Book? book = await bookRepository.GetByIdAsync(command.BookId, cancellationToken);
        if (book is null)
            throw new BookSharingNotFoundException("There is no such book in db.");

        if (book.IsDraft)
            throw new BookSharingValidationException("Book is in draft mode.");

        var userWishListBooks = await wishlistRepository.GetBooksByUserIdAsync(command.UserId, cancellationToken);
        var bookFromWishlist = userWishListBooks!.FirstOrDefault(b => b.Id == command.BookId);
        if (bookFromWishlist is not null)
            throw new BookSharingGenericException("Book is already in wishlist.");

        if (await wishlistRepository.AddBookAsync(command.BookId, command.UserId, cancellationToken) is false)
            throw new BookSharingGenericException("Failed to add book to wishlist.");

        return Unit.Value;
    }
}
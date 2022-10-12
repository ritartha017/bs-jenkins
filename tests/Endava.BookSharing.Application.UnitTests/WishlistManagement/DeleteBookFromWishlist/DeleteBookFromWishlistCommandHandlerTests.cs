using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.WishlistManagement.DeleteBookFromWishlist;
using Endava.BookSharing.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.WishlistManagement.DeleteBookFromWishlist;

public class DeleteBookFromWishlistCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenNoSuchBookIdInWishlist_ThrowsBookSharingNotFoundException(
        Mock<IWishlistRepository> wishlistRepository,
        DeleteBookFromWishlistCommand command)
    {
        var handler = new DeleteBookFromWishlistCommandHandler(wishlistRepository.Object);
        wishlistRepository.Setup(x => x.GetBooksByUserIdAsync(command.UserId, CancellationToken.None))
            .ReturnsAsync(Enumerable.Empty<Book>());

        await Assert.ThrowsAsync<BookSharingNotFoundException>(()
            => handler.Handle(command, CancellationToken.None));
    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenDeleteBookIdForUserIdFails_ThrowsBookSharingGenericException(
        Mock<IWishlistRepository> wishlistRepository,
        DeleteBookFromWishlistCommand command)
    {
        var handler = new DeleteBookFromWishlistCommandHandler(wishlistRepository.Object);
        wishlistRepository.Setup(x => x.GetBooksByUserIdAsync(command.UserId, CancellationToken.None))
            .ReturnsAsync(new List<Book>() { new Book() { Id = command.BookId } });
        wishlistRepository.Setup(x => x.DeleteBookFromUserList(command.BookId, command.UserId, CancellationToken.None))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<BookSharingGenericException>(()
            => handler.Handle(command, CancellationToken.None));
    }


    [Theory]
    [AutoMoqData]
    public async Task Handle_BookARemovedFromWishlist_ShouldNotThrowAsync(
        Mock<IWishlistRepository> wishlistRepository,
        DeleteBookFromWishlistCommand command)
    {
        var handler = new DeleteBookFromWishlistCommandHandler(wishlistRepository.Object);
        wishlistRepository.Setup(x => x.GetBooksByUserIdAsync(command.UserId, CancellationToken.None))
            .ReturnsAsync(new List<Book>() { new Book() { Id = command.BookId } });
        wishlistRepository.Setup(x => x.DeleteBookFromUserList(command.BookId, command.UserId, CancellationToken.None))
            .ReturnsAsync(true);

        Func<Task> result = async () => await handler.Handle(command, CancellationToken.None);

        await result.Should().NotThrowAsync();
    }
}

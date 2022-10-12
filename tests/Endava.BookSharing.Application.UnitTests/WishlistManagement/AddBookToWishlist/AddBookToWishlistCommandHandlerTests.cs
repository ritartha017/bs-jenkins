using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.WishlistManagement.AddBookToWishlist;
using Endava.BookSharing.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.WishlistManagement.AddBookToWishlist;

public class AddBookToWishlistCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenGivenInvalidBookId_ThrowsBookSharingNotFoundException(
        Mock<IBookRepository> bookRepository,
        Mock<IWishlistRepository> wishlistRepository,
        AddBookToWishlistCommand command)
    {
        var handler = new AddBookToWishlistCommandHandler(wishlistRepository.Object, bookRepository.Object);
        bookRepository.Setup(x => x.GetByIdAsync(
            It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Book)null!);

        await Assert.ThrowsAsync<BookSharingNotFoundException>(() 
            => handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenBookIsInDraft_ThrowsBookSharingValidationException(
        Mock<IBookRepository> bookRepository,
        Mock<IWishlistRepository> wishlistRepository,
        AddBookToWishlistCommand command,
        Book book)
    {
        var handler = new AddBookToWishlistCommandHandler(wishlistRepository.Object, bookRepository.Object);
        book.IsDraft = true;
        bookRepository.Setup(x => x.GetByIdAsync(
            It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(book);

        await Assert.ThrowsAsync<BookSharingValidationException>(() 
            => handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_BookNotAddedToWishlist_ThrowsBookSharingGenericException(
         Mock<IBookRepository> bookRepository,
         Mock<IWishlistRepository> wishlistRepository,
         AddBookToWishlistCommand command,
         Book book)
    {
        var handler = new AddBookToWishlistCommandHandler(wishlistRepository.Object, bookRepository.Object);
        bookRepository.Setup(x => x.GetByIdAsync(
            It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(book);
        book.IsDraft = false;
        wishlistRepository.Setup(x => x.AddBookAsync(command.BookId, command.UserId, CancellationToken.None))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<BookSharingGenericException>(()
            => handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenBookIsAlreadyInWishlist_ThrowsBookSharingException(
    Mock<IBookRepository> bookRepository,
    Mock<IWishlistRepository> wishlistRepository,
    AddBookToWishlistCommand command,
    Book book)
    {
        var handler = new AddBookToWishlistCommandHandler(wishlistRepository.Object, bookRepository.Object);
        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(book);
        book.IsDraft = false;
        wishlistRepository.Setup(x => x.GetBooksByUserIdAsync(command.UserId, CancellationToken.None))
            .ReturnsAsync(new List<Book>() { new Book() { Id = command.BookId } });

        await Assert.ThrowsAsync<BookSharingGenericException>(()
            => handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_BookAddedToWishlist_ThrowsBookSharingNotFoundException(
        Mock<IBookRepository> bookRepository,
        Mock<IWishlistRepository> wishlistRepository,
        AddBookToWishlistCommand command,
        Book book)
    {
        var handler = new AddBookToWishlistCommandHandler(wishlistRepository.Object, bookRepository.Object);
        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(book);
        book.IsDraft = false;
        wishlistRepository.Setup(x => x.AddBookAsync(command.BookId, command.UserId, CancellationToken.None))
            .ReturnsAsync(true);

        Func<Task> result = async () => await handler.Handle(command, CancellationToken.None);

        await result.Should().NotThrowAsync();
    }
}

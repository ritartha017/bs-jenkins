using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.BookManagement.DeleteBook;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.DeleteBook;

public class DeleteBookCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task DeleteBook_Handle_ThrowsBookSharingGenericExceptionWhenGivenInvalidID(
        Mock<IBookRepository> bookRepository)
    {
        var user = new AuthenticatedUser("123", new Roles[] { Roles.User });

        var handler = new DeleteBookCommandHandler(bookRepository.Object);
        var command = new DeleteBookCommand(string.Empty, user);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task DeleteBook_Handle_ThrowsBookSharingGenericExceptionWhenBookNotFound(
        Mock<IBookRepository> bookRepository)
    {
        var user = new AuthenticatedUser("123", new Roles[] { Roles.User });

        var handler = new DeleteBookCommandHandler(bookRepository.Object);
        var command = new DeleteBookCommand("book", user);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Book)null!);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }
    [Theory]
    [AutoMoqData]
    public async Task DeleteBook_Handle_ThrowsAccessDeniedExceptionUserNotOwnerAndNotAdminOrSuperAdmin(
        Mock<IBookRepository> bookRepository)
    {
        var user = new AuthenticatedUser("123", new Roles[] { Roles.User });

        var handler = new DeleteBookCommandHandler(bookRepository.Object);
        var command = new DeleteBookCommand("book", user);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book());


        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingAccessDeniedException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task DeleteBook_Handle_ThrowsBookSharingGenericExceptionWhenBookCouldNotBeDeleted(
        Mock<IBookRepository> bookRepository)
    {
        var user = new AuthenticatedUser("123", new Roles[] { Roles.User, Roles.Admin });

        var handler = new DeleteBookCommandHandler(bookRepository.Object);
        var command = new DeleteBookCommand("book", user);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book());

        bookRepository.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task DeleteBook_Handle_ReturnsUnitWithValidDataWhenUserAdminOrSuperAdmin(
        Mock<IBookRepository> bookRepository)
    {
        var user = new AuthenticatedUser("123", new Roles[] { Roles.User, Roles.Admin });

        var handler = new DeleteBookCommandHandler(bookRepository.Object);
        var command = new DeleteBookCommand("book", user);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book());

        bookRepository.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Func<Task> act = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };

        await act.Should().NotThrowAsync();
    }

    [Theory]
    [AutoMoqData]
    public async Task DeleteBook_Handle_ReturnsUnitWithValidDataWhenUserIsOwner(
        Mock<IBookRepository> bookRepository)
    {
        var user = new AuthenticatedUser("123", new Roles[] { Roles.User });

        var handler = new DeleteBookCommandHandler(bookRepository.Object);
        var command = new DeleteBookCommand("book", user);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book() { OwnerId = user.Id });

        bookRepository.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Func<Task> act = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };

        await act.Should().NotThrowAsync();
    }
}

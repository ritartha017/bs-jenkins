using AutoFixture;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.BookManagement.GetBook;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.Models.DtoModels;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Enums;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.GetBook;

public class GetBookCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task HandleOwnerGetBookDetails_IsEditable_ReturnsTrue(
        [Frozen] Mock<IBookRepository> bookRepository,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        GetBookCommand command,
        BookDetailsDto book,
        IFixture fixture)
    {
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        command.AuthenticatedUser = new AuthenticatedUser(book.UploadedBy.Id, new[] { Roles.Admin });

        var handler = fixture.Create<GetBookCommandHandler>();
        bookRepository.Setup(s => s.GetByIdDetailsAsync(
             It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

        await handler.Handle(command, CancellationToken.None);

        Assert.True(book.IsEditable);
    }

    [Theory]
    [AutoMoqData]
    public async Task HandleAdminGetBookDetails_IsEditable_ReturnsTrue(
        [Frozen] Mock<IBookRepository> bookRepository,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        GetBookCommand command,
        BookDetailsDto book,
        IFixture fixture)
    {
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        command.AuthenticatedUser = new AuthenticatedUser("111", new[] { Roles.Admin });

        var handler = fixture.Create<GetBookCommandHandler>();
        bookRepository.Setup(s => s.GetByIdDetailsAsync(
             It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

        await handler.Handle(command, CancellationToken.None);

        Assert.True(book.IsEditable);
    }

    [Theory]
    [AutoMoqData]
    public async Task HandleUserGetBookDetails_IsEditable_ReturnsFalse(
        [Frozen] Mock<IBookRepository> bookRepository,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        GetBookCommand command,
        BookDetailsDto book,
        IFixture fixture)
    {
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        command.AuthenticatedUser = new AuthenticatedUser("111", new[] { Roles.User });

        var handler = fixture.Create<GetBookCommandHandler>();
        bookRepository.Setup(s => s.GetByIdDetailsAsync(
             It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

        await handler.Handle(command, CancellationToken.None);

        Assert.False(book.IsEditable);
    }
    [Theory]
    [AutoMoqData]
    public async Task HandleUserGetBookDetails_BookWithExpectedRating_ReturnsTrue(
        [Frozen] Mock<IBookRepository> bookRepository,
        [Frozen] Mock<IReviewRepository> reviewRepository,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        GetBookCommand command,
        BookDetailsDto book,
        IFixture fixture)
    {
        var expectedRating = 3;
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        command.AuthenticatedUser = new AuthenticatedUser("111", new[] { Roles.User });
        var handler = fixture.Create<GetBookCommandHandler>();
        bookRepository.Setup(s => s.GetByIdDetailsAsync(
             It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

        reviewRepository.Setup(x => x.GetAverageReviewRating(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(expectedRating);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedRating, result.Rating);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenInvalidBookId_ThrowsException(
        GetBookCommand command,
        [Frozen] Mock<IBookRepository> bookRepositoryMock,
        GetBookCommandHandler sut
    )
    {
        bookRepositoryMock.Setup(s => s.GetByIdDetailsAsync(
           It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((BookDetailsDto?)null);

        await Assert.ThrowsAsync<BookSharingGenericException>(()
            => sut.Handle(command, CancellationToken.None));
    }
}
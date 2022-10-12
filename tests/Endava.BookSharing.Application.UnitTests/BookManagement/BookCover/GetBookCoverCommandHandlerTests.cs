using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.BookManagement.BookCover;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.UnitTests.Shared;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.BookCover;


public class GetBookCoverCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handler_ShouldGetBookCoverImage(
        [Frozen] Mock<IBookRepository> bookRepositoryMock,
        [Frozen] Mock<IFileRepository> fileRepositoryMock,
        GetBookCoverQuery command,
        GetBookCoverQueryHandler handler)
    {
        bookRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Entities.Book());

        fileRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Entities.File());

        Func<Task> result = async () => await handler.Handle(command, CancellationToken.None);

        await result.Should().NotThrowAsync();
    }

    [Theory]
    [AutoMoqData]
    public async Task Handler_WhenGivenInvalidBookId_ThrowsBookSharingNotFoundException(
        [Frozen] Mock<IBookRepository> bookRepositoryMock,
        [Frozen] Mock<IFileRepository> fileRepositoryMock)
    {
        var command = new GetBookCoverQuery(String.Empty);
        var handler = new GetBookCoverQueryHandler(bookRepositoryMock.Object, fileRepositoryMock.Object);

        fileRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Entities.File());

        await Assert.ThrowsAsync<BookSharingNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handler_WhenFileIsNull_ShouldReturnNull(
        [Frozen] Mock<IBookRepository> bookRepositoryMock,
        [Frozen] Mock<IFileRepository> fileRepositoryMock,
        GetBookCoverQuery command,
        GetBookCoverQueryHandler handler)
    {
        bookRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Entities.Book());

        fileRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Domain.Entities.File?)null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Null(result);
    }
}


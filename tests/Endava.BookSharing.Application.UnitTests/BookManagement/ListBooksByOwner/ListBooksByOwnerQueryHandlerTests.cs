using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.BookManagement.ListBooksByOwner;
using Endava.BookSharing.Application.Filters;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.ListBooksByOwner;

public class ListBooksByOwnerQueryHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldCallAllMethods(
        [Frozen] Mock<IBookRepository> bookRepository,
        ListBooksByOwnerQuery queryStub,
        int countOfBooks,
        List<Book> books,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        List<ListBooksByOwnerItemsResponse> responses
    )
    {
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        bookRepository.Setup(x => x.GetCountByOwnerIdAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(countOfBooks);
        bookRepository.Setup(x => x.GetByOwnerIdAsync(It.IsAny<string>(),
                It.IsAny<PaginationFilter>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        var sut = new ListBooksByOwnerQueryHandler(bookRepository.Object, bookCoverOptionsMock.Object);
        await sut.Handle(queryStub, CancellationToken.None);
        
        bookRepository.Verify(x =>
            x.GetCountByOwnerIdAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
        bookRepository.Verify(x =>
            x.GetByOwnerIdAsync(It.IsAny<string>(),
                It.IsAny<PaginationFilter>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}
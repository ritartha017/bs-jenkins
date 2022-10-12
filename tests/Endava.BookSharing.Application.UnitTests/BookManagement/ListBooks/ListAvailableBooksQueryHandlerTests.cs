using AutoFixture;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.BookManagement.ListBooks;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Entities.Pagination;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace Endava.BookSharing.Application.UnitTests.BookManagement.ListBooks;

public class ListAvailableBooksQueryHandlerTests
{

    [Theory]
    [AutoMoqData]
    public async Task Handle_PageIsNegatveToPageEqualsOne_ReturnsTrue(
        [Frozen] Mock<IGenreRepository> genreRepository,
        [Frozen] Mock<IBookRepository> bookRepository,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        IFixture fixture)
    {
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        int page = -1;

        int expectedPage = 1;

        var request = new FilterBooksRequest()
        {
            Page = page
        };
        var query = new ListAvailableBooksQuery(request);
        var handler = fixture.Create<ListAvailableBooksQueryHandler>();

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<List<string>>(), CancellationToken.None))
            .ReturnsAsync(new List<Genre>());
        bookRepository.Setup(x => x.GetForPaginationAsync(It.IsAny<FilterBookParams>(), CancellationToken.None))
            .ReturnsAsync(new List<Book>() { new Book() });


        var pagnationList = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(expectedPage, pagnationList.Page);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_PageIsZeroToPageEqualsOne_ReturnsTrue(
        [Frozen] Mock<IGenreRepository> genreRepository,
        [Frozen] Mock<IBookRepository> bookRepository,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        IFixture fixture)
    {
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        int page = 0;

        int expectedPage = 1;

        var request = new FilterBooksRequest()
        {
            Page = page
        };
        var query = new ListAvailableBooksQuery(request);
        var handler = fixture.Create<ListAvailableBooksQueryHandler>();

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<List<string>>(), CancellationToken.None))
            .ReturnsAsync(new List<Genre>());
        bookRepository.Setup(x => x.GetForPaginationAsync(It.IsAny<FilterBookParams>(), CancellationToken.None))
            .ReturnsAsync(new List<Book>() { new Book() });

        var pagnationList = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(expectedPage, pagnationList.Page);
    }
    [Theory]
    [AutoMoqData]
    public async Task Handle_CoverUrlStartsWithTheSameHost_ReturnsTrue(
        [Frozen] Mock<IGenreRepository> genreRepository,
        [Frozen] Mock<IBookRepository> bookRepository,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        IFixture fixture)
    {
        bookCoverOptions.Url = "http://test.local/cover/{0}";
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        int page = 1;

        var request = new FilterBooksRequest()
        {
            Page = page
        };
        var query = new ListAvailableBooksQuery(request);
        var handler = fixture.Create<ListAvailableBooksQueryHandler>();

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<List<string>>(), CancellationToken.None))
            .ReturnsAsync(new List<Genre>());
        bookRepository.Setup(x => x.GetForPaginationAsync(It.IsAny<FilterBookParams>(), CancellationToken.None))
            .ReturnsAsync(new List<Book>() { new Book() { Id = "asb", Title = "abc" } });

        var pagnationList = await handler.Handle(query, CancellationToken.None);

        var paginationItems = pagnationList.Items as List<PaginationBookItem>;

        Assert.Equal("http://test.local/cover/asb", paginationItems![0].Cover);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenNoBooksAvailable_ThrowsBookSharingNotFoundException(
        [Frozen] Mock<IGenreRepository> genreRepository,
        [Frozen] Mock<IBookRepository> bookRepository,
        BookCoverOptions bookCoverOptions,
        [Frozen] Mock<IOptions<BookCoverOptions>> bookCoverOptionsMock,
        IFixture fixture)
    {
        bookCoverOptionsMock.SetupGet(x => x.Value).Returns(bookCoverOptions);
        int page = 1;

        var request = new FilterBooksRequest()
        {
            Page = page
        };
        var query = new ListAvailableBooksQuery(request);
        var handler = fixture.Create<ListAvailableBooksQueryHandler>();

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<List<string>>(), CancellationToken.None))
                    .ReturnsAsync(new List<Genre>());
        bookRepository.Setup(x => x.GetForPaginationAsync(It.IsAny<FilterBookParams>(), CancellationToken.None))
            .ReturnsAsync(new List<Book>());

        var exception = await Record.ExceptionAsync(() => handler.Handle(query, CancellationToken.None));

        Assert.IsType<BookSharingNotFoundException>(exception);
    }
}

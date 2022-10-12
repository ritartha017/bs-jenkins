using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.ReviewManagement.GetReviews;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Endava.BookSharing.Application.Models.DtoModels;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.ReviewManagement.GetReviews;

public class GetReviewQueryHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handler_WhenGivenInvalidBookId_ThrowsBookSharingNotFoundException(
        [Frozen] Mock<IBookRepository> bookRepositoryMock,
        [Frozen] Mock<IReviewRepository> reviewRepositoryMock)
    {
        var query = new GetReviewsQuery(string.Empty, It.IsAny<int>());
        var handler = new GetReviewsQueryHandler(bookRepositoryMock.Object, reviewRepositoryMock.Object);

        reviewRepositoryMock.Setup(f => f.GetPaginationBookReviews(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ReviewListDto?>());


        await Assert.ThrowsAsync<BookSharingNotFoundException>(() => handler.Handle(query, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenGivenAllValidData_NotThrowAsync(
        [Frozen] Mock<IBookRepository> bookRepositoryMock,
        [Frozen] Mock<IReviewRepository> reviewRepositoryMock,
        GetReviewsQuery query,
        GetReviewsQueryHandler handler)
    {
        bookRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<string>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book());

        reviewRepositoryMock.Setup(b => b.GetPaginationBookReviews
            (It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ReviewListDto?>());

        Func<Task> result = async () => await handler.Handle(query, CancellationToken.None);

        await result.Should().NotThrowAsync();
    }
}


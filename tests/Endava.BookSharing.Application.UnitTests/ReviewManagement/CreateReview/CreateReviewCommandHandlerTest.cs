using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.BookFeedback;
using Endava.BookSharing.Domain.Entities;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.ReviewManagement.CreateReview
{
    public class CreateReviewCommandHandlerTest
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_WhenBookDoesNotExist_ShouldThrowException(
        [Frozen] Mock<IBookRepository> bookRepositoryMock,
        [Frozen] Mock<IReviewRepository> reviewRepositoryMock,
        CreateReviewCommandHandler sut,
        CreateReviewCommand command)
        {
            bookRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Book?)null);

            await Assert.ThrowsAsync<BookSharingGenericException>(() =>
            sut.Handle(command, CancellationToken.None));

            reviewRepositoryMock.Verify(r =>
            r.CreateReviewAsync(It.IsAny<Review>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_WhenBookExists_ShouldInvokeCreateReviewAsyncMethod(
        [Frozen] Mock<IBookRepository> bookRepositoryMock,
        [Frozen] Mock<IReviewRepository> reviewRepositoryMock,
        CreateReviewCommandHandler sut,
        CreateReviewCommand command,
        Book book)
        {
            bookRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            await sut.Handle(command, CancellationToken.None);

            reviewRepositoryMock.Verify(r =>
            r.CreateReviewAsync(It.IsAny<Review>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
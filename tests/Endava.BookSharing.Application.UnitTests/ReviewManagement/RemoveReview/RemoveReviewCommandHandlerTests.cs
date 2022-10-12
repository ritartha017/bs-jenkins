using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.FeedbackManagement.RemoveFeedback;
using Endava.BookSharing.Application.UnitTests.Shared;
using FluentAssertions;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.FeedbackManagement.RemoveFeedback;

public class RemoveReviewCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldCallRemoveFeedbackAsync(
        [Frozen] Mock<IReviewRepository> reviewRepository,
        RemoveReviewCommandHandler sut,
        string feedbackId)
    {
        reviewRepository.Setup(x => x.RemoveByIdAsync(It.IsAny<string>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new RemoveReviewCommand(feedbackId);

        await sut.Handle(command, CancellationToken.None);

        reviewRepository.Verify(x =>
            x.RemoveByIdAsync(feedbackId,It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_WithNotValidId_ShouldThrowNotFound(
        [Frozen] Mock<IReviewRepository> reviewRepository,
        RemoveReviewCommandHandler sut,
        string feedbackId)
    {
        reviewRepository.Setup(x => x.RemoveByIdAsync(It.IsAny<string>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new RemoveReviewCommand(feedbackId);

        Func<Task> act = async () => await sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BookSharingNotFoundException>();
    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_WithValidId_ShouldNotThrowNotFound(
        [Frozen] Mock<IReviewRepository> reviewRepository,
        RemoveReviewCommandHandler sut,
        string feedbackId)
    {
        reviewRepository.Setup(x => x.RemoveByIdAsync(It.IsAny<string>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new RemoveReviewCommand(feedbackId);

        Func<Task> act = async () => await sut.Handle(command, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }
}
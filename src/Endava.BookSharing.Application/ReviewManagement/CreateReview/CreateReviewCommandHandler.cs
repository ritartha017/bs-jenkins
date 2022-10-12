using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.BookFeedback;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Unit>
{
    private readonly IReviewRepository reviewRepository;
    private readonly IBookRepository bookRepository;
    private readonly IDateTimeProvider dateTimeProvider;

    public CreateReviewCommandHandler(IReviewRepository reviewRepository, IBookRepository bookRepository, IDateTimeProvider dateTimeProvider)
    {
        this.reviewRepository = reviewRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.bookRepository = bookRepository;
    }

    public async Task<Unit> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(command.BookId, cancellationToken);
        if (book is null)
        {
            throw new BookSharingGenericException("Book with specified id not found.");
        }

        var review = new Review()
        {
            Id = Guid.NewGuid().ToString(),
            BookId = command.BookId,
            Content = command.Content,
            PostedByUserId = command.UserId,
            PostedAt = dateTimeProvider.Now,
            Rating = command.Rating,
            Title = command.Title
        };

        await reviewRepository.CreateReviewAsync(review, cancellationToken);

        return Unit.Value;
    }
}
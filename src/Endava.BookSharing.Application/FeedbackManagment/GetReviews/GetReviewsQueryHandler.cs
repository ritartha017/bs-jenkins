using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Entities.Pagination;
using MediatR;

namespace Endava.BookSharing.Application.ReviewManagement.GetReviews;

public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, PaginationList<GetReviewsQueryItemsResponse>>
{
    private readonly IBookRepository bookRepository;
    private readonly IReviewRepository reviewRepository;
    private readonly ICollection<GetReviewsQueryItemsResponse> _responses;

    public GetReviewsQueryHandler(IBookRepository bookRepository, 
        IReviewRepository reviewRepository)
    {
        this.bookRepository = bookRepository;
        this.reviewRepository = reviewRepository;
        _responses = new List<GetReviewsQueryItemsResponse>();
    }


    public async Task<PaginationList<GetReviewsQueryItemsResponse>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.BookId, cancellationToken);

        if (book is null)
        {
            throw new BookSharingNotFoundException("Invalid Book ID");
        }

        var feedbacksCount = await reviewRepository.GetBookReviewsCount(request.BookId);
        var totalPages = (int)Math.Ceiling(Convert.ToDouble(feedbacksCount) / Convert.ToDouble(AppConsts.ReviewsPerPage));

        var feedbacks = await reviewRepository.GetPaginationBookReviews(request.BookId, request.Page,
            AppConsts.ReviewsPerPage, cancellationToken);

        foreach (var feedback in feedbacks)
        {
            _responses.Add(new GetReviewsQueryItemsResponse
            {
                FeedbackId = feedback.FeedbackId,
                Title = feedback.Title,
                Content = feedback.Content,
                PostedAt = feedback.PostedAt,
                PostedBy = feedback.PostedByUser,
                Rating = feedback.Rating
            });
        }

        var paginationList = new PaginationList<GetReviewsQueryItemsResponse>
        {
            Page = request.Page,
            PerPage = AppConsts.ReviewsPerPage,
            TotalPages = totalPages,
            Items = _responses
        };

        return paginationList;
    }

}
using Endava.BookSharing.Domain.Entities.Pagination;
using MediatR;

namespace Endava.BookSharing.Application.ReviewManagement.GetReviews;

public class GetReviewsQuery : IRequest<PaginationList<GetReviewsQueryItemsResponse>>
{
    public GetReviewsQuery(string bookId, int page)
    {
        BookId = bookId;
        Page = page;
    }
    public string BookId { get; }
    public int Page { get; }
}


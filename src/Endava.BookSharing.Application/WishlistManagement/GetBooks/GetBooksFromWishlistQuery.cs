using Endava.BookSharing.Domain.Entities.Pagination;
using MediatR;

namespace Endava.BookSharing.Application.WishlistManagement.GetBooks
{
    public class GetBooksFromWishlistQuery : IRequest<PaginationList<PaginationBookItem>>
    {
        public GetBooksFromWishlistQuery(int page, string userId)
        {
            Page = page;
            UserId = userId;
        }

        public int Page { get; }
        public string UserId { get; }
    }
}
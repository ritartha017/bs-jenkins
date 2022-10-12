using Endava.BookSharing.Application.Models.DtoModels;
using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Abstract;

public interface IReviewRepository
{
    Task<bool> CreateReviewAsync(Review review, CancellationToken cancellationToken);
    Task<ICollection<ReviewListDto?>> GetPaginationBookReviews(string bookId, int page, int perPage, CancellationToken cancellationToken);
    Task<double> GetAverageReviewRating(string bookId, CancellationToken cancellationToken);
    Task<int> GetBookReviewsCount(string bookId);
    Task<bool> RemoveByIdAsync(string reviewId, CancellationToken cancellationToken);
}

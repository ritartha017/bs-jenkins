using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Domain.Entities.Pagination;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.ListBooks;

public class ListAvailableBooksQuery : IRequest<PaginationList<PaginationBookItem>>
{
    public ListAvailableBooksQuery(FilterBooksRequest request)
    {
        Page = request.Page;
        Language = request.Language;
        if (request.Genres is not null)
        {
            Genres = request.Genres.Split(',').ToList();
        }

        if (request.RatingMin is null)
        {
            RatingMin = AppConsts.MinReviewRating;
        }
        else
        {
            IsRatingSpecified = true;
            RatingMin = (int)request.RatingMin;
        }

        if (request.RatingMax is null)
        {
            RatingMax = AppConsts.MaxReviewRating;
        }
        else
        {
            IsRatingSpecified = true;
            RatingMax = (int)request.RatingMax;
        }

        Author = request.Author;
    }
    public int Page { get; }

    public string? Language { get; }
    public ICollection<string> Genres { get; } = new List<string>();
    public int RatingMin { get; }
    public int RatingMax { get; }
    public string? Author { get; }
    public bool IsRatingSpecified { get; set; } = false;
}

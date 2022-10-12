using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Models.DtoModels;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext dbContext;

    public ReviewRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> RemoveByIdAsync(string reviewId, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId, cancellationToken);

        if (entity is null)
        {
            return false;
        }

        dbContext.Set<ReviewDb>().Remove(entity);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> CreateReviewAsync(Review review, CancellationToken cancellationToken)
    {
        var book = await dbContext.Books.FindAsync(new object?[] {review.BookId}, cancellationToken: cancellationToken);
        if (book is null) return false;

        var user = await dbContext.Users.FindAsync(new object?[] {review.PostedByUserId},
            cancellationToken: cancellationToken);
        if (user is null) return false;

        var reviewDb = new ReviewDb()
        {
            Id = review.Id,
            Book = book,
            Content = review.Content,
            PostedAt = review.PostedAt.ToUniversalTime(),
            PostedBy = user,
            Rating = review.Rating,
            Title = review.Title
        };
        await dbContext.Reviews.AddAsync(reviewDb, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<double> GetAverageReviewRating(string bookId, CancellationToken cancellationToken)
    {
        double? rating = await dbContext.Reviews
            .AsNoTracking()
            .Where(x => x.Book.Id == bookId)
            .AverageAsync(x => (int?)x.Rating, cancellationToken);

        return rating ?? 0;
    }

    public async Task<int> GetBookReviewsCount(string bookId)
    {
        return await dbContext.Reviews.Where(x => x.Book.Id == bookId).CountAsync();
    }

    public async Task<ICollection<ReviewListDto?>> GetPaginationBookReviews(string bookId, int page, int perPage,
        CancellationToken cancellationToken)
    {
        var feedbacks = await dbContext.Reviews
            .AsNoTracking()
            .Where(x => x.Book.Id == bookId)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .Select(x => new ReviewListDto
            {
                BookId = x.Book.Id,
                FeedbackId = x.Id,
                Title = x.Title,
                Content = x.Content,
                Rating = x.Rating,
                PostedAt = x.PostedAt.ToUniversalTime(),
                PostedByUser = new UserDto
                {
                    UserId = x.PostedBy.Id,
                    FullName = x.PostedBy.FirstName + " " + x.PostedBy.LastName,
                    UserName = x.PostedBy.UserName
                }
            })
            .ToListAsync(cancellationToken);

        return feedbacks;
    }
}
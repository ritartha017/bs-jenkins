using AutoMapper;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public WishlistRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<bool> AddBookAsync(string bookId, string userId, CancellationToken cancellationToken)
    {
        var bookDb = await dbContext.Books
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id == bookId, cancellationToken: cancellationToken);

        if (bookDb is null)
            return false;

        var bookToWishlist = new WishlistDb() { UserId = userId, BookId = bookId };
        await dbContext.Wishlists.AddAsync(bookToWishlist, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteBookFromUserList(string bookId, string userId, CancellationToken cancellationToken)
    {
        WishlistDb? bookFromWishlist = await dbContext.Wishlists
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BookId == bookId 
             && x.UserId == userId, cancellationToken: cancellationToken);

        if (bookFromWishlist is null)
        {
            return false;
        }

        dbContext.Wishlists.Remove(bookFromWishlist);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<Book>> GetBooksByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        var wishlistDb = await dbContext.Wishlists.AsNoTracking().Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);

        if (wishlistDb.Count <= 0) return Enumerable.Empty<Book>();

        IEnumerable<string> booksIds = wishlistDb.Select(x => x.BookId).ToList();

        var books = await dbContext.Books
            .Where(b => booksIds.Any(x => b.Id == x))
            .Select(b => new Book()
            {
                Id = b.Id,
                Title = b.Title,
                OwnerId = b.OwnerId,
                AuthorId = b.AuthorId,
                IsDraft = b.IsDraft,
                PublicationDate = b.PublicationDate,
                Genres = mapper.Map<ICollection<Genre>>(b.Genres),
                LanguageId = b.LanguageId,
                CoverId = b.CoverId,
            }).ToListAsync(cancellationToken);
        return books;
    }

    public async Task<IEnumerable<Book>> GetForPaginationBooksAsync(string userId, int skipNoOfItems, int takeNoOfItems, CancellationToken cancellationToken)
    {
        var wishlistDb = await dbContext.Wishlists.AsNoTracking().Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);

        if (wishlistDb.Count <= 0) return Enumerable.Empty<Book>();

        IEnumerable<string> booksIds = wishlistDb.Select(x => x.BookId).ToList();

        var books = await dbContext.Books
            .AsNoTracking()
            .Where(b => booksIds.Any(x => b.Id == x))
            .Where(x => !x.IsDraft)
            .OrderByDescending(x => x.PublicationDate)
            .Skip(skipNoOfItems)
            .Take(takeNoOfItems)
            .Select(b => new Book()
            {
                Id = b.Id,
                Title = b.Title
            }).ToListAsync(cancellationToken);
        return books;
    }
}
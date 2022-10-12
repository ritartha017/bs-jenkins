using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.BookManagement.ListBooks;
using Endava.BookSharing.Application.Extensions;
using Endava.BookSharing.Application.Filters;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.Models.DtoModels;
using Endava.BookSharing.Application.Models.Response;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IModelMapper _mapper;

    public BookRepository(ApplicationDbContext applicationDbContext, IModelMapper mapper)
    {
        _mapper = mapper;
        dbContext = applicationDbContext;
    }
    public Task<int> GetCountByOwnerIdAsync(string ownerId, CancellationToken cancellationToken)
    {
        return Task.FromResult(dbContext.Books.Count(x => x.OwnerId == ownerId));
    }

    public async Task<ICollection<Book>> GetByOwnerIdAsync(string ownerId, PaginationFilter filter, CancellationToken cancellationToken)
    {
        var pagedBooksDb = await dbContext.Books
            .Where(x => x.OwnerId == ownerId)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        var pagedBooks = _mapper.Map<List<Book>>(pagedBooksDb);
        return pagedBooks;
    }

    public async Task<bool> CreateAsync(Book book, CancellationToken cancellationToken)
    {
        var cover = await dbContext.Files
            .FindAsync(new object?[] { book.CoverId }, cancellationToken: cancellationToken);
        if (cover is null) return false;

        var author = await dbContext.Authors
            .FindAsync(new object?[] { book.AuthorId }, cancellationToken: cancellationToken);
        if (author is null) return false;

        var language = await dbContext.Languages
            .FindAsync(new object?[] { book.LanguageId }, cancellationToken: cancellationToken);
        if (language is null) return false;

        var owner = await dbContext.Users
            .FindAsync(new object?[] { book.OwnerId }, cancellationToken: cancellationToken);
        if (owner is null) return false;

        var genresIds = book.Genres.Select(c => c.Id).ToList();
        var genres = dbContext.Genres.Where(t => genresIds.Contains(t.Id)).ToList();
        if (genres is null || genresIds.Count != book.Genres.Count)
            return false;

        var appBook = new BookDb()
        {
            Id = book.Id,
            Title = book.Title,
            IsDraft = book.IsDraft,
            PublicationDate = book.PublicationDate.ToUniversalTime(),
            Cover = cover,
            Author = author,
            Language = language,
            Owner = owner,
            Genres = genres,
        };

        await dbContext.AddAsync(appBook, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(string bookId, CancellationToken cancellationToken)
    {
        var book = await dbContext.Books
            .FindAsync(new object?[] { bookId }, cancellationToken: cancellationToken);

        if (book is null)
            return false;

        dbContext.Books.Remove(book);

        var rows = await dbContext.SaveChangesAsync(cancellationToken);

        return rows != 0;
    }

    public async Task<Book?> GetByIdAsync(string bookId, CancellationToken cancellationToken)
    {
        var bookDb = await dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == bookId, cancellationToken: cancellationToken);

        if (bookDb is null)
            return null;

        var book = new Book()
        {
            Id = bookId,
            Title = bookDb.Title,
            OwnerId = bookDb.OwnerId,
            AuthorId = bookDb.AuthorId,
            IsDraft = bookDb.IsDraft,
            PublicationDate = bookDb.PublicationDate,
            LanguageId = bookDb.LanguageId,
            CoverId = bookDb.CoverId
        };
        return book;
    }

    public async Task<BookDetailsDto?> GetByIdDetailsAsync(string bookId, CancellationToken cancellationToken)
    {
        BookDb? bookDb = await dbContext.Books
            .Where(x => x.Id == bookId)
            .Include(x => x.Cover)
            .Include(x => x.Owner)
            .Include(x => x.Author)
            .Include(x => x.Language)
            .Include(x => x.Genres)
            .FirstOrDefaultAsync(x => x.Id == bookId,
            cancellationToken: cancellationToken);


        if (bookDb is null)
            return null;

        var book = new BookDetailsDto()
        {
            Title = bookDb.Title,
            UploadedBy = _mapper.Map<UserResponse>(bookDb.Owner),
            Author = _mapper.Map<AuthorResponse>(bookDb.Author),
            Language = _mapper.Map<Language>(bookDb.Language),
            Genres = _mapper.Map<ICollection<Genre>>(bookDb.Genres),
            PublicationDate = bookDb.PublicationDate
        };
        return book;
    }

    public async Task<bool> UpdateAsync(Book newBookData, bool deleteCover, CancellationToken cancellationToken)
    {
        var cover = await dbContext.Files
            .FindAsync(new object?[] { newBookData.CoverId }, cancellationToken: cancellationToken);
        if (cover is null) return false;

        var author = await dbContext.Authors
            .FindAsync(new object?[] { newBookData.AuthorId }, cancellationToken: cancellationToken);
        if (author is null) return false;

        var language = await dbContext.Languages
            .FindAsync(new object?[] { newBookData.LanguageId }, cancellationToken: cancellationToken);
        if (language is null) return false;

        var genresIds = newBookData.Genres.Select(c => c.Id).ToList();
        var genres = dbContext.Genres.Where(t => genresIds.Contains(t.Id)).ToList();
        if (genres is null || genresIds.Count != newBookData.Genres.Count)
            return false;

        var bookFromDb = await dbContext.Books
            .Where(x => x.Id == newBookData.Id)
            .Include(x => x.Owner)
            .Include(x => x.Genres)
            .Include(x => x.Cover)
            .FirstOrDefaultAsync(cancellationToken);

        if (bookFromDb == null) return false;

        for (int i = 0; i < bookFromDb.Genres.Count; i++)
        {
            bookFromDb.Genres.ElementAt(i).Books.Remove(bookFromDb);
        }
        await dbContext.SaveChangesAsync(cancellationToken);

        if (bookFromDb.Cover is not null && deleteCover)
        {
            dbContext.Files.Remove(bookFromDb.Cover);
        }

        bookFromDb.Title = newBookData.Title;
        bookFromDb.IsDraft = newBookData.IsDraft;
        bookFromDb.PublicationDate = newBookData.PublicationDate.ToUniversalTime();
        bookFromDb.Cover = cover;
        bookFromDb.Author = author;
        bookFromDb.Language = language;
        bookFromDb.Genres = genres;

        var rows = await dbContext.SaveChangesAsync(cancellationToken);

        return rows != 0;
    }

    public async Task<bool> DeleteCoverAsync(string bookId, CancellationToken cancellationToken)
    {
        var book = await dbContext.Books
            .Where(x => x.Id == bookId)
            .Include(x => x.Cover)
            .FirstOrDefaultAsync(cancellationToken);

        if (book is null)
            return false;

        if (book.CoverId is null && book.Cover is null)
        {
            return true;
        }

        book.CoverId = null;

        if (book.Cover is not null)
        {
            dbContext.Files.Remove(book.Cover);
        }

        var rows = await dbContext.SaveChangesAsync(cancellationToken);

        return rows != 0;
    }

    public async Task<ICollection<Book>> GetForPaginationAsync(FilterBookParams filters, CancellationToken cancellationToken)
    {
        IQueryable<BookDb> query = dbContext.Books
            .AsNoTracking()
            .Where(x => !x.IsDraft)
            .Include(x => x.Genres)
            .Include(x => x.Author)
            .Include(x => x.Reviews);

        if (filters.Language is not null)
        {
            query = query.Where(x => x.LanguageId == filters.Language);
        }

        if (filters.Author is not null)
        {
            query = query.Where(x => x.Author.FullName.ToLower().Contains(filters.Author));
        }

        var booksDb = await query
            .OrderByDescending(x => x.PublicationDate)
            .ToListAsync(cancellationToken);

        if (filters.Genres.Any())
        {
            booksDb = booksDb.Where(b => filters.Genres.All(f => b.Genres.Any(x => x.Id == f))).ToList();
        }

        if (filters.IsRatingSpecified)
        {
            booksDb = booksDb.Where(x => (
                    (!x.Reviews.Any() ? 0 : x.Reviews.Average(r => r.Rating))
                    .IsBetween(filters.RatingMin, filters.RatingMax)
                )).ToList();
        }



        var books = new List<Book>();

        foreach (var book in booksDb)
        {
            books.Add(new Book()
            {
                Id = book.Id,
                Title = book.Title
            });
        }
        return books;
    }

}
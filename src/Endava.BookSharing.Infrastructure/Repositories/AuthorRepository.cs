using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IModelMapper mapper;

    public AuthorRepository(ApplicationDbContext dbContext, IModelMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<Author?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        AuthorDb? authorDb = await dbContext.Authors
            .Include(a => a.AddedBy)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return authorDb is null ? null : new Author()
        {
            Id = authorDb.Id,
            FullName = authorDb.FullName,
            IsApproved = authorDb.IsApproved,
            AddedByUserId = authorDb.AddedBy.Id,
        };
    }

    public async Task<List<Author>> ListAllAsync(CancellationToken cancellationToken)
    {
        var authorsDb = await dbContext.Authors.Where(author =>
            author.IsApproved).OrderBy(x => x.FullName).ToListAsync(cancellationToken);
        var authors = mapper.Map<List<Author>>(authorsDb);

        return authors;
    }

    public async Task<Author?> GetByNameAsync(string authorName, CancellationToken cancellationToken)
    {
        var authorDb = await dbContext.Set<AuthorDb>()
            .Include(author => author.AddedBy)
            .AsNoTracking()
            .FirstOrDefaultAsync(author => author.FullName == authorName, cancellationToken);

        if (authorDb is null)
        {
            return null;
        }

        var author = new Author
        {
            Id = authorDb.Id,
            FullName = authorDb.FullName,
            AddedByUserId = authorDb.AddedBy.Id,
            IsApproved = authorDb.IsApproved
        };

        return author;
    }

    public async Task<string?> CreateAsync(Author author, CancellationToken cancellationToken)
    {
        var addedBy = await dbContext.Users
            .FirstAsync(entity => entity.Id == author.AddedByUserId, cancellationToken);

        var authorDb = new AuthorDb
        {
            Id = Guid.NewGuid().ToString(),
            FullName = author.FullName,
            AddedBy = addedBy,
            IsApproved = author.IsApproved
        };

        await dbContext.Set<AuthorDb>()
            .AddAsync(authorDb, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return authorDb.Id;
    }

    public async Task<bool> IsNameUniqueAsync(string authorName, CancellationToken cancellationToken)
    {
        var result = await dbContext.Authors.FirstOrDefaultAsync(a =>
            a.FullName == authorName, cancellationToken);

        return result is null;
    }
}
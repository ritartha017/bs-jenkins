using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class LanguageRepository : ILanguageRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IModelMapper mapper;

    public LanguageRepository(ApplicationDbContext dbContext, IModelMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<Language?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        LanguageDb? languageDb = await dbContext.Languages
            .FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

        return languageDb is null ? null : new Language()
        {
            Id = languageDb.Id,
            Name = languageDb.Name
        };
    }

    public async Task<Language?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        LanguageDb? languageDb = await dbContext.Languages.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        if (languageDb is null)
            return null;

        Language language = new Language()
        {
            Id = languageDb.Id,
            Name = languageDb.Name
        };

        return language;
    }

    public async Task<List<Language>> ListAllAsync(CancellationToken cancellationToken)
    {
        var languagesDb = await dbContext.Languages.ToListAsync(cancellationToken);
        var languages = mapper.Map<List<Language>>(languagesDb);

        return languages;
    }
}


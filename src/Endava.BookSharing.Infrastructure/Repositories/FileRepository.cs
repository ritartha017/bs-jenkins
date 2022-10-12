using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Domain.Abstractions;
using Endava.BookSharing.Infrastructure.Persistence;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly ApplicationDbContext dbContext;
    public FileRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<string?> CreateAsync(Domain.Entities.File file, CancellationToken cancellationToken)
    {
        FileDb newFileDb = new()
        {
            Id = file.Id,
            ContentType = file.Data.ContentType,
            Raw = file.Data.Raw,
        };
        await dbContext.AddAsync(newFileDb, cancellationToken);
        var rows = await dbContext.SaveChangesAsync(cancellationToken);

        return rows != 0 ? newFileDb.Id : null!;
    }

    public async Task<Domain.Entities.File?> GetByBytesAsync(IFileData file, CancellationToken cancellationToken)
    {
        FileDb? fileFromDb = await dbContext.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Raw == file.Raw, cancellationToken);

        return fileFromDb is null ? null : new Domain.Entities.File()
        {
            Id = fileFromDb.Id,
            Data = new ImageData(fileFromDb.ContentType, fileFromDb.Raw),
        };
    }

    public async Task<Domain.Entities.File?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var file = await dbContext.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return file is null ? null : new Domain.Entities.File()
        {
            Data = new ImageData(file.ContentType, file.Raw)

        };
    }
}

using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using MediatR;

namespace Endava.BookSharing.Application.FileManagement.CreateFile;

public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, string>
{
    private readonly IFileRepository fileRepository;

    public CreateFileCommandHandler(IFileRepository fileRepository)
    {
        this.fileRepository = fileRepository;
    }

    public async Task<string> Handle(CreateFileCommand request, CancellationToken cancellationToken)
    {
        if (request.ContentType is null || request.Raw is null)
            throw new BookSharingGenericException("File type and file data must be specified");

        var cover = new Domain.Entities.File()
        {
            Id = Guid.NewGuid().ToString(),
            Data = request
        };
        var coverId = await fileRepository.CreateAsync(cover, cancellationToken);

        if (coverId is null)
            throw new BookSharingGenericException("Failed to create file.");

        return coverId;
    }
}

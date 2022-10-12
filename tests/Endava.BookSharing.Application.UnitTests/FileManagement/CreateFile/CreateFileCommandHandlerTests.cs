using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.FileManagement.CreateFile;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.FileManagement.CreateFile;

public class CreateFileCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task CreateFile_Handle_ThrowsBookSharingGenericExceptionWhenContentTypeIsNull(
        Mock<IFileRepository> fileRepository)
    {
        var handler = new CreateFileCommandHandler(fileRepository.Object);
        var command = new CreateFileCommand(null!, new byte[1]);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }
    [Theory]
    [AutoMoqData]
    public async Task CreateFile_Handle_ThrowsBookSharingGenericExceptionWhenRawIsNull(
        Mock<IFileRepository> fileRepository)
    {
        var handler = new CreateFileCommandHandler(fileRepository.Object);
        var command = new CreateFileCommand("image/png", null!);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }
    [Theory]
    [AutoMoqData]
    public async Task CreateFile_Handle_ThrowsBookSharingGenericExceptionWhenContentTypeAndRawIsNull(
        Mock<IFileRepository> fileRepository)
    {
        var handler = new CreateFileCommandHandler(fileRepository.Object);
        var command = new CreateFileCommand(null!, null!);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }
    [Theory]
    [AutoMoqData]
    public async Task CreateFile_Handle_ThrowsBookSharingGenericExceptionWhenCreationFails(
        Mock<IFileRepository> fileRepository)
    {
        fileRepository.Setup(x => x.CreateAsync(It.IsAny<File>(), CancellationToken.None))
            .ReturnsAsync((string)null!);

        var handler = new CreateFileCommandHandler(fileRepository.Object);
        var command = new CreateFileCommand("image/png", new byte[1]);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }
    [Theory]
    [AutoMoqData]
    public async Task CreateFile_Handle_ReturnsCoverId(
        Mock<IFileRepository> fileRepository)
    {
        var expectedCoverId = "1234";

        fileRepository.Setup(x => x.CreateAsync(It.IsAny<File>(), CancellationToken.None))
            .ReturnsAsync(expectedCoverId);

        var handler = new CreateFileCommandHandler(fileRepository.Object);
        var command = new CreateFileCommand("image/png", new byte[1]);

        var returnedId = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedCoverId, returnedId);
    }
}

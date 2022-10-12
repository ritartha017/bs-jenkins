using System;
using System.Runtime.CompilerServices;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.UnitTests.Shared;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.FileManagement.DeleteFile;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.FileManagement.DeleteFile;

public class DeleteFileCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_AuthorDeleteCover_ReturnsTrue(
        DeleteFileCommand command,
        [Frozen] Mock<IBookRepository> iBookRepositoryMock,
        DeleteFileCommandHandler sut,
        Book book
        )
    {
        iBookRepositoryMock.Setup(s =>
                s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        
        iBookRepositoryMock.Setup(s =>
                s.DeleteCoverAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        command.UserId = book.AuthorId;
        
        await sut.Handle(command, CancellationToken.None);

        iBookRepositoryMock.Verify(s =>
            s.DeleteCoverAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        iBookRepositoryMock.Verify(s =>
            s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_AdminDeleteCover_ReturnsTrue(
        DeleteFileCommand command,
        [Frozen] Mock<IBookRepository> iBookRepositoryMock,
        DeleteFileCommandHandler sut,
        Book book
    )
    {
        iBookRepositoryMock.Setup(s =>
                s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        
        iBookRepositoryMock.Setup(s =>
                s.DeleteCoverAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        command.UserRoles = new[] { Roles.Admin };
        
        await sut.Handle(command, CancellationToken.None);

        iBookRepositoryMock.Verify(s =>
            s.DeleteCoverAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        iBookRepositoryMock.Verify(s =>
            s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_NotOwnerAndUserRole_DeleteCover_ThrowsException(
        DeleteFileCommand command,
        [Frozen] Mock<IBookRepository> iBookRepositoryMock,
        DeleteFileCommandHandler sut,
        Book book
    )
    {
        iBookRepositoryMock.Setup( s =>
                s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        
        iBookRepositoryMock.Setup( s =>
                s.DeleteCoverAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        command.UserRoles = new[] { Roles.User };
        command.UserId = "123";
        book.OwnerId = "234";
        
        await Assert.ThrowsAsync<BookSharingAccessDeniedException>(() =>
            sut.Handle(command, CancellationToken.None));

    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_DeleteCover_BookDoesNotExist_ThrowsException(
        DeleteFileCommand command,
        [Frozen] Mock<IBookRepository> iBookRepositoryMock,
        DeleteFileCommandHandler sut
    )
    {
        iBookRepositoryMock.Setup( s =>
                s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?) null);

        await Assert.ThrowsAsync<BookSharingGenericException>(() =>
            sut.Handle(command, CancellationToken.None));

    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_DeleteCover_ThrowsException_CouldNotBeDeleted(
        DeleteFileCommand command,
        [Frozen] Mock<IBookRepository> iBookRepositoryMock,
        DeleteFileCommandHandler sut,
        Book book
    )
    {
        iBookRepositoryMock.Setup( s =>
                s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        
        iBookRepositoryMock.Setup( s =>
                s.DeleteCoverAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<BookSharingGenericException>(() =>
            sut.Handle(command, CancellationToken.None));

    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_UserIsBookOwner_NoBookCover_DeleteCoverAsync_ShoudlNotBeCalled(
        DeleteFileCommand command,
        [Frozen] Mock<IBookRepository> iBookRepositoryMock,
        DeleteFileCommandHandler sut,
        Book book
    )
    {
        iBookRepositoryMock.Setup( s =>
                s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        
        command.UserRoles = new[] { Roles.User };
        command.UserId = book.OwnerId;
        book.CoverId = null;
        await sut.Handle(command, CancellationToken.None);
        
        iBookRepositoryMock.Verify(s => s.DeleteCoverAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

    }
}
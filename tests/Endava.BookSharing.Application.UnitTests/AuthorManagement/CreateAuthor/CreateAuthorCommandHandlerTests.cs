using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.AuthorManagement.CreateAuthor;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.AuthorManagement.CreateAuthor;

public class CreateAuthorCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldCallCreateAsync(
        [Frozen] Mock<IAuthorRepository> authorRepository,
        [Frozen] Mock<IUserRepository> userRepository,
        CreateAuthorCommandHandler sut,
        string fullName,
        string addedById,
        Author author,
        User user)
    {
        authorRepository.Setup(x => x.GetByNameAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);
        userRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        authorRepository.Setup(x => x.CreateAsync(It.IsAny<Author>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(author.Id);

        var command = new CreateAuthorCommand(fullName, addedById);

        await sut.Handle(command, CancellationToken.None);

        authorRepository.Verify(x => x.CreateAsync(It.IsAny<Author>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnCreatedAuthorId(
        [Frozen] Mock<IAuthorRepository> authorRepository,
        [Frozen] Mock<IUserRepository> userRepository,
        CreateAuthorCommandHandler sut,
        string fullName,
        string addedById,
        Author author,
        User user)
    {
        authorRepository.Setup(x => x.GetByNameAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);
        userRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        authorRepository.Setup(x => x.CreateAsync(It.IsAny<Author>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(author.Id);

        var command = new CreateAuthorCommand(fullName, addedById);

        var createdId = await sut.Handle(command, CancellationToken.None);

        Assert.Equal(createdId, author.Id);
    }
}
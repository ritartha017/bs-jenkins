using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.AuthorManagement.GetAuthorsList;
using Endava.BookSharing.Application.UnitTests.Profiles;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.AuthorManagement.GetAuthorsList;

public class GetAuthorsListQueryHandlerTests : IClassFixture<MappingTestsFixture>
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldCallListAllAsync(
        [Frozen] Mock<IAuthorRepository> authorRepository,
        GetAuthorsListQueryHandler sut,
        List<Author> authors)
    {
        authorRepository.Setup(x => x.ListAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(authors);

        var command = new GetAuthorsListQuery();

        await sut.Handle(command, CancellationToken.None);

        authorRepository.Verify(x =>
            x.ListAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
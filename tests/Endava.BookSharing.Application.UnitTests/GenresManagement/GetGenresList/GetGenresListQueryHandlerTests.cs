using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.GenresManagement.GetGenresList;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.GenresManagement.GetGenresList;

public class GetGenresListQueryHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldCallListAllAsync(
        [Frozen] Mock<IGenreRepository> genreRepository,
        GetGenresListQueryHandler sut,
        List<Genre> genres)
    {
        genreRepository.Setup(x => x.ListAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(genres);

        var command = new GetGenresListQuery();

        await sut.Handle(command, CancellationToken.None);

        genreRepository.Verify(x =>
            x.ListAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnLanguagesList(
        [Frozen] Mock<IGenreRepository> genreRepository,
        GetGenresListQueryHandler sut,
        List<Genre> genres)
    {
        genreRepository.Setup(x => x.ListAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(genres);

        var command = new GetGenresListQuery();

        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Equal(genres, result);
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.LanguageManagement.GetLanguagesList;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.LanguageManagement.GetLanguagesList;

public class GetLanguagesListQueryHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldCallListAllAsync(
        [Frozen] Mock<ILanguageRepository> languageRepository,
        GetLanguagesListQueryHandler sut,
        List<Language> languages)
    {
        languageRepository.Setup(x => x.ListAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages);

        var command = new GetLanguagesListQuery();

        await sut.Handle(command, CancellationToken.None);

        languageRepository.Verify(x =>
            x.ListAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnLanguagesList(
        [Frozen] Mock<ILanguageRepository> languageRepository,
        GetLanguagesListQueryHandler sut,
        List<Language> languages)
    {
        languageRepository.Setup(x => x.ListAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages);

        var command = new GetLanguagesListQuery();

        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Equal(languages, result);
    }
}
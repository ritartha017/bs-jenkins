using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models.Mappers;

public class ModelMapperTests
{
    [Theory]
    [AutoMoqData]
    public void ShouldMapObjectToAnotherObject(
        LanguageDb languageDb,
        [Frozen] Mock<IModelMapper> _mapper)
    {
        _mapper.Setup(x => x.Map<LanguageDb>(It.IsAny<Language>()))
            .Returns(languageDb);

        var entity = new Language();

        var result = _mapper.Object.Map<LanguageDb>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<LanguageDb>();
    }
}
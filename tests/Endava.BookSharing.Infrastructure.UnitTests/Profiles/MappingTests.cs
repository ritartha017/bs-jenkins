using AutoMapper;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence.Models;
using Endava.BookSharing.Infrastructure.UnitTests.Shared;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Infrastructure.UnitTests.Profiles;

public class MappingTests : IClassFixture<MappingTestsFixture>
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests(MappingTestsFixture fixture)
    {
        _configuration = fixture.ConfigurationProvider;
        _mapper = fixture.Mapper;
    }

    [Theory]
    [AutoMoqData]
    public void ShouldMapLanguageToLanguageDb(
        LanguageDb entity)
    {
        var result = _mapper.Map<LanguageDb>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<LanguageDb>();
        result.Should().BeEquivalentTo(entity);
    }

    [Theory]
    [AutoMoqData]
    public void ShouldMapLanguageDbToLanguage(
        LanguageDb entity)
    {
        var result = _mapper.Map<Language>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<Language>();
        result.Should().BeEquivalentTo(entity);
    }
    
    [Theory]
    [AutoMoqData]
    public void ShouldMapAuthorToAuthorDb(
        Author entity)
    {
        var result = _mapper.Map<AuthorDb>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<AuthorDb>();
        result.Should().BeEquivalentTo(entity, opt =>
            opt.ExcludingMissingMembers());
    }

    [Theory]
    [AutoMoqData]
    public void ShouldMapAuthorDbToAuthor(
        Author entity)
    {
        var result = _mapper.Map<Author>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<Author>();
        result.Should().BeEquivalentTo(entity);
    }

    [Theory]
    [AutoMoqData]
    public void ShouldMapGenreToGenreDb(
        Genre entity)
    {
        var result = _mapper.Map<GenreDb>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<GenreDb>();
        result.Should().BeEquivalentTo(entity);
    }

    [Theory]
    [AutoMoqData]
    public void ShouldMapGetGenreDbToGenre(
            Genre entity)
    {
        var result = _mapper.Map<Genre>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<Genre>();
        result.Should().BeEquivalentTo(entity);
    }
}

using AutoMapper;
using Endava.BookSharing.Application.AuthorManagement.GetAuthorsList;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Profiles;

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
    public void ShouldMapAuthorToGetAuthorsListVm(
        Author entity)
    {
        var result = _mapper.Map<GetAuthorsListItemResponse>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<GetAuthorsListItemResponse>();
        result.Should().BeEquivalentTo(entity, opt =>
            opt.ExcludingMissingMembers());
    }

    [Theory]
    [AutoMoqData]
    public void ShouldMapGetAuthorsListVmToAuthor(
        GetAuthorsListItemResponse entity)
    {
        var result = _mapper.Map<Author>(entity);

        result.Should().NotBeNull();
        result.Should().BeOfType<Author>();
        result.Should().BeEquivalentTo(entity, opt =>
            opt.ExcludingMissingMembers());
    }
}

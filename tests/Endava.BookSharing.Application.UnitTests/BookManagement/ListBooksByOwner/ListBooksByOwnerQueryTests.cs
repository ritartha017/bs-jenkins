using Endava.BookSharing.Application.BookManagement.ListBooksByOwner;
using Endava.BookSharing.Application.Filters;
using Endava.BookSharing.Application.UnitTests.Shared;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.ListBooksByOwner;

public class ListBooksByOwnerQueryTests
{
    [Theory]
    [AutoMoqData]
    public void Constructor_ShouldSetProperties(string ownerId, PaginationFilter filter)
    {
        var result = new ListBooksByOwnerQuery(ownerId, filter);

        Assert.Equal(ownerId, result.OwnerId);
        result.Should().BeEquivalentTo(filter,
            opt =>
                opt.ComparingByMembers<PaginationFilter>()
                    .ExcludingMissingMembers());
    }
}
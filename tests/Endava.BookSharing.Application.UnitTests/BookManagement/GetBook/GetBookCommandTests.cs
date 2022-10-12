using Endava.BookSharing.Application.BookManagement.GetBook;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Domain.Enums;
using Endava.BookSharing.Application.UnitTests.Shared;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.GetBook;

public class GetBookCommandTests
{
    [Theory]
    [AutoMoqData]
    public void CreateObject_WithValidParameter_ValuesHaveBeenAssignedToFields(string bookId, AuthenticatedUser authenticatedUser)
    {
        var result = new GetBookCommand(bookId, authenticatedUser);

        result.Should().NotBeNull();
        Assert.Equal(bookId, result.BookId);
        Assert.Equal(authenticatedUser, result.AuthenticatedUser);
        
    }
}
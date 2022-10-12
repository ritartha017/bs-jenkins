using Endava.BookSharing.Application.FileManagement.DeleteFile;
using Endava.BookSharing.Domain.Enums;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UnitTests.Shared;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.FileManagement.DeleteFile;

public class DeleteFileCommandTests
{
    [Theory]
    [AutoMoqData]
    public void CreateObject_WithValidParameter_ValuesHaveBeenAssignedToFields(string bookId, string userId, Roles[] role)
    {
        var result = new DeleteFileCommand(bookId, userId, role);

        result.Should().NotBeNull();
        Assert.Equal(bookId, result.BookId);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(role, result.UserRoles);
    }
}
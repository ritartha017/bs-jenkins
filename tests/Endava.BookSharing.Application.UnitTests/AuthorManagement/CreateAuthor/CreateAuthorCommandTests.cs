using Endava.BookSharing.Application.AuthorManagement.CreateAuthor;
using Endava.BookSharing.Application.UnitTests.Shared;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.AuthorManagement.CreateAuthor;

public class CreateAuthorCommandTests
{
    [Theory]
    [AutoMoqData]
    public void Constructor_ShouldSetProperties(string fullName, string addedById)
    {
        var result = new CreateAuthorCommand(fullName, addedById);

        Assert.Equal(fullName, result.FullName);
        Assert.Equal(addedById, result.AddedById);
    }
}
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.AssignRole;
using Endava.BookSharing.Domain.Enums;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.AssignRole;

public class AssignRoleCommandTests
{
    [Theory]
    [AutoMoqData]
    public void Constructor_ShouldSetProperties(string id, Roles role)
    {
        var result = new AssignRoleCommand(id, role);

        Assert.Equal(id, result.Id);
    }
}
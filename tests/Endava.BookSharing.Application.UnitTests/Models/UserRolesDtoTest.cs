using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.UnitTests.Shared;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models
{
    public class UserRolesDtoTest
    {
        [Theory]
        [AutoMoqData]
        public void CreateObject_WithValidParameter_ValuesHaveBeenAssignedToFields(
        string userId, IEnumerable<string> userRoles)
        {
            var result = new UserRolesDto(userId, userRoles);

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.UserRoles.Should().BeEquivalentTo(userRoles);
        }
    }
}
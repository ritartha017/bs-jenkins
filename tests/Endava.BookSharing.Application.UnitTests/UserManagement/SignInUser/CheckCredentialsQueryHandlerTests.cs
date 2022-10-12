using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.UserSignIn;
using Endava.BookSharing.Application.UserManagement.UserSignUp;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.SignInUser
{
    public class CheckCredentialsQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_WithValidQuery_CallsMethodValidateUserCredentials(
        [Frozen] Mock<IIdentityService> identityServiceMock,
        CheckCredentialsQueryHandler sut,
        CheckCredentialsQuery query)
        {
            await sut.Handle(query, CancellationToken.None);
            identityServiceMock.Verify(static r => r.ValidateUserCredentialsAsync
            (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
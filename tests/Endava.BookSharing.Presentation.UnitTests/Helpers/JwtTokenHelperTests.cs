//using AutoFixture;
//using AutoFixture.Xunit2;
//using Endava.BookSharing.Application.Abstract;
//using Endava.BookSharing.Infrastructure.Settings;
//using Endava.BookSharing.Presentation.Helpers;
//using Endava.BookSharing.Presentation.UnitTests.Shared;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using Moq;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Threading.Tasks;
//using Xunit;

//namespace Endava.BookSharing.Presentation.UnitTests.Helpers
//{
//    public class JwtTokenHelperTests
//    {
//        [Theory]
//        [AutoMoqData]
//        public async Task CreateAuthToken_ShouldReturnValidJwt(
//            [Frozen] Mock<IOptions<TokenSettings>> tokenSettingsMock,
//            TokenSettings tokenSettings,
//            [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock,
//            IFixture fixture)
//        {
//            string expected = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
//                "eyJ1c2VySWQiOiIyZmM4MmE0YS02ZTM2LTQ3OTEtYjk4Yi0wYzgwNWFjYjZjMTgiLCJyb2xlcy" +
//                "I6IlVzZXIiLCJuYmYiOjE2NTQyNDk2MDIsImV4cCI6MTY1NDI1MTQwMiwiaWF0IjoxNjU0MjQ5" +
//                "NjAyLCJpc3MiOiJodHRwOi8vYm9va3NoYXJpbmcubG9jYWwiLCJhdWQiOiJodHRwOi8vYm9va3" +
//                "NoYXJpbmcubG9jYWwifQ.-iL9lX-x2IRz2Lprygcpp2nOH5iKXV36GLzSGjADkm8";
//            string userId = "2fc82a4a-6e36-4791-b98b-0c805acb6c18";
//            string[] roles = new string[] { "User" };
//            tokenSettings.Secret = "veryVerySuperSecretKey";
//            tokenSettings.ExpirationInMinutes = 30;
//            tokenSettings.Audience = "http://booksharing.local";
//            tokenSettings.Issuer = "http://booksharing.local";
//            dateTimeProviderMock.SetupGet(x => x.Now).Returns(new System.DateTime(2022, 06, 03, 12, 46, 42));
//            tokenSettingsMock.SetupGet(x => x.Value)
//                .Returns(tokenSettings);

//            var sut = fixture.Create<JwtTokenHelper>();

//            var result = sut.CreateAuthToken(userId, roles);

//            Assert.Equal(expected, result);
//        }
//    }
//}
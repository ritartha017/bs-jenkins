using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Application.UnitTests.Shared.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models.Validators
{
    public class UserSignInValidatorTests
    {
        #region Validations

        private ValidationResult? RunValidation(UserSignInRequest userSignInRequest)
        {
            var validator = new UserSignInValidator();
            var result = validator.Validate(userSignInRequest);
            return result;
        }

        #endregion Validations

        #region UsernameEmptyOrNotTests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_Username_EmptyOrNullUsername_DoesntAllowEmpty(string username)
        {
            var result = RunValidation(new UserSignInRequest() { UserName = username });
            result.AssertFailedValidation("The username cannot be empty");
        }

        [Theory]
        [InlineData("Alexei")]
        public void Validate_Username_UsernameAlexei_AcceptsBecauseNotEmpty(string username)
        {
            var result = RunValidation(new UserSignInRequest() { UserName = username });
            result.HasNotErrorMessage("The username cannot be empty");
        }

        #endregion UsernameEmptyOrNotTests

        #region PasswordEmptyOrNotTests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_Password_PasswordNullOrEmpty_Rejects(string password)
        {
            var result = RunValidation(new UserSignInRequest() { Password = password });
            result.AssertFailedValidation("The password cannot be empty");
        }

        [Theory]
        [InlineData("password")]
        [InlineData("bbb\nbbb")]
        public void Validate_Password_PasswordWithSomeCharacters(string password)
        {
            var result = RunValidation(new UserSignInRequest() { Password = password });
            result.HasNotErrorMessage("The password cannot be empty");
        }

        #endregion PasswordEmptyOrNotTests
    }
}
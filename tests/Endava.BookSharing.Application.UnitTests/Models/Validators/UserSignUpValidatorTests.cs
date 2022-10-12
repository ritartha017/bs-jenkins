using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Application.UnitTests.Shared.Extensions;
using FluentValidation.Results;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models.Validators
{
    public class UserSignUpValidatorTests
    {
        #region Validations

        private ValidationResult? RunValidation(UserSignUpRequest userSignUpRequest)
        {
            var validator = new UserSignUpValidator();
            var result = validator.Validate(userSignUpRequest);
            return result;
        }

        #endregion Validations

        #region UsernameEmptyOrNotTests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_Username_EmptyOrNullUsername_DoesntAllowEmpty(string username)
        {
            var result = RunValidation(new UserSignUpRequest() { Username = username });
            result.AssertFailedValidation("The username cannot be empty");
        }

        [Theory]
        [InlineData("Nikita")]
        public void Validate_Username_UsernameNikita_AcceptsBecauseNotEmpty(string username)
        {
            var result = RunValidation(new UserSignUpRequest() { Username = username });
            result.HasNotErrorMessage("The username cannot be empty");
        }

        #endregion

        #region UsernameLengthTests

        [Theory]
        [InlineData("bbb")]
        [InlineData("bbbbbbb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        public void Validate_Username_UsernameWithAllowedLength_Accepts(string username)
        {
            var result = RunValidation(new UserSignUpRequest() { Username = username });
            result.HasNotErrorMessage("The username must have at least 3 characters and not more than 30");
        }


        [Theory]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        [InlineData("bb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        public void Validate_Username_UsernameWithNotallowedLengthCharacters_DoesntAccept(string username)
        {
            var result = RunValidation(new UserSignUpRequest() { Username = username });
            result.AssertFailedValidation("The username must have at least 3 characters and not more than 30");
        }

        #endregion

        #region UsernnameRegularExpressionTests

        [Theory]
        [InlineData("user")]
        [InlineData("User")]
        [InlineData("user2")]
        [InlineData("user.name")]
        public void Validate_UserName_ValidSymbols_ShouldReturnSuccessValidation(string username)
        {
            var result = RunValidation(new UserSignUpRequest() { Username = username });
            result.HasNotErrorMessage("Invalid username");
        }

        [Theory]
        [InlineData("юзер")]
        [InlineData("us\u0435r")]
        [InlineData("us\ter")]
        [InlineData("us er")]
        [InlineData("us\ner")]
        [InlineData("NiKi\tta")]
        public void Validate_UserName_InValidSymbols_ShouldReturnFailedValidation(string username)
        {
            var result = RunValidation(new UserSignUpRequest() { Username = username });
            result.AssertFailedValidation("Invalid username");
        }

        #endregion

        #region EmailEmptyOrNotTests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_Email_EmptyOrNullEmail_Rejects(string email)
        {
            var result = RunValidation(new UserSignUpRequest() { Email = email });
            result.AssertFailedValidation("The email cannot be empty");
        }

        [Theory]
        [InlineData("MaybeThis@is.Email")]
        [InlineData("MaybeThisIsEmail")]
        public void Validate_Email_NotEmptyEmail_Accepts(string email)
        {
            var result = RunValidation(new UserSignUpRequest() { Email = email });
            result.HasNotErrorMessage("The email cannot be empty");
        }

        #endregion

        #region EmailRegularExpressionTests

        [Theory]
        [InlineData("maybeThisIsEmail")]
        [InlineData("maybeThis.IsEmail")]
        [InlineData("maybe This.IsEmail")]
        [InlineData("maybe\tThis.IsEmail")]
        [InlineData("maybeThis..IsEmail")]
        [InlineData("maybeThis@@IsEmail")]
        [InlineData("MaybeThisIs@.Email")]
        [InlineData("maybeThis@IsEmail.")]
        [InlineData("maybeThis@IsEmail")]
        [InlineData("----maybeThis@Is.Email///")]
        [InlineData("maybe###$This@Is.Email")]
        public void Validate_Email_InvalidEmail_Rejects(string email)
        {
            var result = RunValidation(new UserSignUpRequest() { Email = email });
            result.AssertFailedValidation("Invalid email address");
        }

        [Theory]
        [InlineData("MaybeThis@Is.Email")]
        [InlineData("MaybeThis123@Is.Email")]
        [InlineData("MaybeThis@Is123.Email")]
        [InlineData("MaybeThis@Is.Email123")]
        [InlineData("MaybeThis1@Is2.Email3")]
        [InlineData(".....maybeThis@Is.Email")]
        [InlineData(".....maybeThis@Is.Email.....")]
        public void Validate_Email_ValidEmail_Accepts(string email)
        {
            var result = RunValidation(new UserSignUpRequest() { Email = email });
            result.HasNotErrorMessage("Invalid email address");
        }

        #endregion

        #region FirstNameEmptyOrNotTests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_FirstName_EmptyOrNullFirstName_Rejects(string firstName)
        {
            var result = RunValidation(new UserSignUpRequest() { FirstName = firstName });
            result.AssertFailedValidation("The first name cannot be empty");
        }

        [Theory]
        [InlineData("Nikita")]
        [InlineData("Nikita123")]
        public void Validate_FirstName_FirstNameWithSomeSymbols_Accepts(string firstName)
        {
            var result = RunValidation(new UserSignUpRequest() { FirstName = firstName });
            result.HasNotErrorMessage("The first name cannot be empty");
        }

        #endregion

        #region FirstNameLengthTests

        [Theory]
        [InlineData("bb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        public void Validate_FirstName_FirstNameWithInvalidLength_Rejects(string firstName)
        {
            var result = RunValidation(new UserSignUpRequest() { FirstName = firstName });
            result.AssertFailedValidation("The first name must have at least 3 characters and not more than 30");
        }

        [Theory]
        [InlineData("bbb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        [InlineData("bbbbbbbbbbbbbbb")]
        public void Validate_FirstName_NameWithValidLength_Accepts(string firstName)
        {
            var result = RunValidation(new UserSignUpRequest() { FirstName = firstName });
            result.HasNotErrorMessage("The first name must have at least 3 characters and not more than 30");
        }

        #endregion

        #region FirstNameRegularExressionTests

        [Theory]
        [InlineData("n1k1ta")]
        [InlineData("nikit@")]
        [InlineData("nikit\ta")]
        public void Validate_FirstName_InvalidFirstName_Rejects(string firstName)
        {
            var result = RunValidation(new UserSignUpRequest() { FirstName = firstName });
            result.AssertFailedValidation("The first name must have only latin letters");
        }

        [Theory]
        [InlineData("nikita")]
        [InlineData("Nikita")]
        [InlineData("NikiTa")]
        [InlineData("NIKITA")]
        public void Validate_FirstName_ValidFirstName_Accepts(string firstName)
        {
            var result = RunValidation(new UserSignUpRequest() { FirstName = firstName });
            result.HasNotErrorMessage("The first name must have only latin letters");
        }

        #endregion

        #region LastNameEmptyOrNotTests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_LastName_emptyOrNullLastnName_Rejects(string lastName)
        {
            var result = RunValidation(new UserSignUpRequest() { LastName = lastName });
            result.AssertFailedValidation("The last name cannot be empty");
        }

        [Theory]
        [InlineData("Nikitov")]
        [InlineData("Nikitov123")]
        public void Validate_LastName_LastNameWithSomeCharacters_Accept(string lastName)
        {
            var result = RunValidation(new UserSignUpRequest() { LastName = lastName });
            result.HasNotErrorMessage("The last name cannot be empty");
        }

        #endregion

        #region LastNameLengthTests

        [Theory]
        [InlineData("bb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        public void Validate_LastName_LastNameWithInvalidLength_Rejects(string lastName)
        {
            var result = RunValidation(new UserSignUpRequest() { LastName = lastName });
            result.AssertFailedValidation("The last name must have at least 3 characters and not more than 30");
        }

        [Theory]
        [InlineData("bbb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        [InlineData("bbbbbbbbbbbbbbbbb")]
        public void Validate_LastName_LastNameWithValidLength_Accepts(string lastName)
        {
            var result = RunValidation(new UserSignUpRequest() { LastName = lastName });
            result.HasNotErrorMessage("The last name must have at least 3 characters and not more than 30");
        }

        #endregion

        #region LastNameRegularExpressionTests

        [Theory]
        [InlineData("123")]
        [InlineData("bbb123")]
        [InlineData("bbb\tbbb")]
        [InlineData("жгыф")]
        public void Validate_LastName_LastNameWithInvalidCharacters_Reject(string lastName)
        {
            var result = RunValidation(new UserSignUpRequest() { LastName = lastName });
            result.AssertFailedValidation("The last name must have only latin letters");
        }

        [Theory]
        [InlineData("Nikitov")]
        [InlineData("NikItOv")]
        [InlineData("NIKITOV")]
        public void Validate_LastName_LastNameWithValidCharacters_Accepts(string lastName)
        {
            var result = RunValidation(new UserSignUpRequest() { LastName = lastName });
            result.HasNotErrorMessage("The last name must have only latin letters");
        }

        #endregion

        #region PasswordEmptyOrNotTests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_Password_PasswordNullOrEmpty_Rejects(string password)
        {
            var result = RunValidation(new UserSignUpRequest() { Password = password });
            result.AssertFailedValidation("The password cannot be empty");
        }

        [Theory]
        [InlineData("password")]
        [InlineData("bbb\nbbb")]
        public void Validate_Password_PasswordWithSomeCharacters(string password)
        {
            var result = RunValidation(new UserSignUpRequest() { Password = password });
            result.HasNotErrorMessage("The password cannot be empty");
        }

        #endregion

        #region PasswordLengthTests

        [Theory]
        [InlineData("bb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        public void Validate_Password_PasswordWithInvalidLength_Rejects(string password)
        {
            var result = RunValidation(new UserSignUpRequest() { Password = password });
            result.AssertFailedValidation("The password must have at least 3 characters and not more than 30");
        }

        [Theory]
        [InlineData("bbb")]
        [InlineData("bbbbbbbbbbbbb")]
        [InlineData("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")]
        public void Validate_Password_PasswordWithValidLength_Accepts(string password)
        {
            var result = RunValidation(new UserSignUpRequest() { Password = password });
            result.HasNotErrorMessage("The password must have at least 3 characters and not more than 30");
        }

        #endregion

        #region PasswordRegularExpressionTests

        [Theory]
        [InlineData("пароль")]
        [InlineData("pass\u0435word")]
        [InlineData("pass\tword")]
        [InlineData("pass word")]
        [InlineData("pass\nword")]
        public void Validate_Password_InvalidPassword_Rejects(string password)
        {
            var result = RunValidation(new UserSignUpRequest() { Password = password });
            result.AssertFailedValidation("Invalid password");
        }

        [Theory]
        [InlineData("password")]
        [InlineData("Password")]
        [InlineData("password2")]
        public void Validate_Password_ValidPassword_Accepts(string password)
        {
            var result = RunValidation(new UserSignUpRequest() { Password = password });
            result.HasNotErrorMessage("Invalid password");
        }

        #endregion
    }
}
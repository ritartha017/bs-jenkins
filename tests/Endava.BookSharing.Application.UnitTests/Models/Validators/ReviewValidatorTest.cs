using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Application.UnitTests.Shared.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models.Validators
{
    public class ReviewValidatorTest
    {
        #region Validations

        private ValidationResult? RunValidation(CreateReviewRequest createReviewRequest)
        {
            var validator = new CreateReviewValidator();
            var result = validator.Validate(createReviewRequest);
            return result;
        }

        #endregion Validations

        #region BookIdEmptyOrNotTests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_BookId_EmptyOrNullBookId_DoesntAllowEmpty(string bookId)
        {
            var result = RunValidation(new CreateReviewRequest() { BookId = bookId });
            result.AssertFailedValidation("The bookId cannot be empty.");
        }

        [Theory]
        [InlineData("someId123")]
        public void Validate_BokId_NotEmptyBookId_AcceptsBecauseNotEmpty(string bookId)
        {
            var result = RunValidation(new CreateReviewRequest() { BookId = bookId });
            result.HasNotErrorMessage("The bookId cannot be empty.");
        }

        #endregion BookIdEmptyOrNotTests

        #region TitleEmptyOrNotTests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_Title_EmptyOrNullTitle_DoesntAllowEmpty(string title)
        {
            var result = RunValidation(new CreateReviewRequest() { Title = title });
            result.AssertFailedValidation("The title cannot be empty.");
        }

        [Theory]
        [InlineData("someTitle")]
        public void Validate_Title_NotEmptyTitle_AcceptsBecauseNotEmpty(string title)
        {
            var result = RunValidation(new CreateReviewRequest() { Title = title });
            result.HasNotErrorMessage("The title cannot be empty.");
        }

        #endregion TitleEmptyOrNotTests

        #region TitleInRangeFrom_5_To_100_OrNotTests

        [Theory]
        [InlineData("     titl")]
        [InlineData("  t     ")]
        [InlineData("t")]
        [InlineData("ttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttt")]
        public void Validate_Title_WithOutOfRangeTitle_DoesntAllow(string title)
        {
            var result = RunValidation(new CreateReviewRequest() { Title = title });
            result.AssertFailedValidation("Title must be in range from 5 to 100 characters.");
        }

        [Theory]
        [InlineData("someTitle")]
        [InlineData("  MyTitle ")]
        public void Validate_Title_InRangeFrom_5_To_100_Title_Accepts(string title)
        {
            var result = RunValidation(new CreateReviewRequest() { Title = title });
            result.HasNotErrorMessage("Title must be in range from 5 to 100 characters.");
        }

        #endregion TitleInRangeFrom_5_To_100_OrNotTests

        #region TitleMatchesRequirementsOrNotTests

        [Theory]
        [InlineData("123dsfsdfdfsd")]
        [InlineData("ывфыывадывлаы")]
        [InlineData("@#$dsfssdfsk")]
        [InlineData("()*&^%$#@!~`")]
        [InlineData("128128314")]
        public void Validate_Title_WithNotAllowedCharactersTitle_DoesntAllow(string title)
        {
            var result = RunValidation(new CreateReviewRequest() { Title = title });
            result.AssertFailedValidation("Title must contein only latin letters and punctuation marks.");
        }

        [Theory]
        [InlineData("someTitle")]
        [InlineData("someTitle,!:?.'")]
        [InlineData("   someTitle.")]
        public void Validate_Title_WithAllowedCharacters_Accepts(string title)
        {
            var result = RunValidation(new CreateReviewRequest() { Title = title });
            result.HasNotErrorMessage("Title must contein only latin letters and punctuation marks.");
        }

        #endregion TitleMatchesRequirementsOrNotTests

        #region ContentInRangeFrom_10_To_500_OrNotTests

        [Theory]
        [InlineData("ttttttttt")]
        [InlineData("  t     ")]
        [InlineData("t")]
        [InlineData("ttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "ttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" +
            "tttttttttttt")]
        public void Validate_Content_WithOutOfRangeContent_DoesntAllow(string content)
        {
            var result = RunValidation(new CreateReviewRequest() { Content = content });
            result.AssertFailedValidation("Content must be in range from 10 to 500 characters.");
        }

        [Theory]
        [InlineData("my conteeeeeeeeeeeeeeeeeeent")]
        [InlineData("  conteeeeeeeeeent  ")]
        public void Validate_Content_InRangeFrom_10_To_500_Content_Accepts(string content)
        {
            var result = RunValidation(new CreateReviewRequest() { Content = content });
            result.HasNotErrorMessage("Content must be in range from 10 to 500 characters.");
        }

        #endregion ContentInRangeFrom_10_To_500_OrNotTests

        #region TitleMatchesRequirementsOrNotTests

        [Theory]
        [InlineData("123dsfsdfdfsd")]
        [InlineData("ывфыывадывлаы")]
        [InlineData("@#$dsfssdfsk")]
        [InlineData("()*&^%$#@!~`")]
        [InlineData("128128314")]
        public void Validate_Content_WithNotAllowedCharactersContent_DoesntAllow(string content)
        {
            var result = RunValidation(new CreateReviewRequest() { Content = content });
            result.AssertFailedValidation("Content must contein only latin letters and punctuation marks.");
        }

        [Theory]
        [InlineData("someContent")]
        [InlineData("someContent,!:?.'")]
        [InlineData("   someContent.  ")]
        public void Validate_Content_WithAllowedCharacters_Accepts(string content)
        {
            var result = RunValidation(new CreateReviewRequest() { Content = content });
            result.HasNotErrorMessage("Content must contein only latin letters and punctuation marks.");
        }

        #endregion TitleMatchesRequirementsOrNotTests

        #region RatingInRangeFrom_1_To_5_OrNotTests

        [Theory]
        [InlineData(6)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_Rating_WithOutOfRangeRating_DoesntAllow(int rating)
        {
            var result = RunValidation(new CreateReviewRequest() { Rating = rating });
            result.AssertFailedValidation("Rating must be in range from 1 to 5.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void Validate_Rating_WithRatingInRangeFrom_1_To_5_Accepts(int rating)
        {
            var result = RunValidation(new CreateReviewRequest() { Rating = rating });
            result.HasNotErrorMessage("Rating must be in range from 1 to 5.");
        }

        #endregion RatingInRangeFrom_1_To_5_OrNotTests
    }
}
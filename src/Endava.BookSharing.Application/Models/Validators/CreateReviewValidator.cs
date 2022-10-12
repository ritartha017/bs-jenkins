using Endava.BookSharing.Application.Models.Requests;
using FluentValidation;

namespace Endava.BookSharing.Application.Models.Validators
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
    {
        public CreateReviewValidator()
        {
            RuleFor(model => model.BookId)
                .NotEmpty()
                .WithMessage("The bookId cannot be empty.");

            RuleFor(model => model.Title)
                .NotEmpty()
                .WithMessage("The title cannot be empty.");
            RuleFor(model => model.Title)
                .Must(TitleLengthExcludingLeadingAndTrailingSpaces)
                .WithMessage("Title must be in range from 5 to 100 characters.");
            RuleFor(model => model.Title)
                .Matches("^[a-zA-Z\\s\\.,!\\?:\"'\\-]*$")
                .WithMessage("Title must contein only latin letters and punctuation marks.");

            RuleFor(model => model.Content)
                .Must(ContentLengthExcludingLeadingAndTrailingSpaces)
                .WithMessage("Content must be in range from 10 to 500 characters.");
            RuleFor(model => model.Content)
                .Matches("^[a-zA-Z\\s\\.,!\\?:\"'\\-]*$")
                .WithMessage("Content must contein only latin letters and punctuation marks.");

            RuleFor(model => model.Rating)
                 .NotEmpty()
                 .WithMessage("The rating cannot be empty.");
            RuleFor(model => model.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be in range from 1 to 5.");
        }

        public static bool TitleLengthExcludingLeadingAndTrailingSpaces(string title)
        {
            if (title is null) return true;
            var trimedTitle = title.Trim();
            if (trimedTitle.Length < 5 || trimedTitle.Length > 100) return false;
            return true;
        }

        public static bool ContentLengthExcludingLeadingAndTrailingSpaces(string? content)
        {
            if (content is null) return true;
            var trimedContent = content.Trim();
            if (trimedContent.Length < 10 || trimedContent.Length > 500) return false;
            return true;
        }
    }
}
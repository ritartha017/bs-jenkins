using Endava.BookSharing.Application.Models.Requests;
using FluentValidation;

namespace Endava.BookSharing.Application.Models.Validators;

public class FilterBooksValidator : AbstractValidator<FilterBooksRequest>
{
    public FilterBooksValidator()
    {
        RuleFor(model => model.RatingMax)
            .Must(BeValidRating)
            .When(model => model.RatingMax != null)
            .WithMessage($"Rating must be equal or between {AppConsts.MinReviewRating} and {AppConsts.MaxReviewRating}");
        RuleFor(model => model.RatingMin)
            .Must(BeValidRating)
            .When(model => model.RatingMin != null)
            .WithMessage($"Rating must be equal or between {AppConsts.MinReviewRating} and {AppConsts.MaxReviewRating}");
    }

    private static bool BeValidRating(int? value)
        => value! >= AppConsts.MinReviewRating && value! <= AppConsts.MaxReviewRating;
}

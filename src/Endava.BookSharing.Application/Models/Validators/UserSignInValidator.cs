using Endava.BookSharing.Application.Models.Requests;
using FluentValidation;

namespace Endava.BookSharing.Application.Models.Validators;

public class UserSignInValidator : AbstractValidator<UserSignInRequest>
{
    public UserSignInValidator()
    {
        RuleFor(model => model.UserName)
                .NotEmpty()
                .WithMessage("The username cannot be empty");

        RuleFor(model => model.Password)
                .NotEmpty()
                .WithMessage("The password cannot be empty");
    }
}
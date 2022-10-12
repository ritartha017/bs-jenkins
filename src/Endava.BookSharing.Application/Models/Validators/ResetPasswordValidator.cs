using Endava.BookSharing.Application.Models.Requests;
using FluentValidation;
namespace Endava.BookSharing.Application.Models.Validators;
public class ResetPasswordValidator : AbstractValidator<UserResetPasswordRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty()
            .WithMessage("The email cannot be empty");
        RuleFor(model => model.Email)
            .Matches("^[\\da-zA-Z.]+@[\\da-zA-Z.]+\\.[\\da-zA-Z.]+$")
            .WithMessage("Invalid email address. Alphanumeric characters, dots, '@'");
        RuleFor(model => model.Password)
            .NotEmpty()
            .WithMessage("Invalid Password. The password cannot be empty");
        RuleFor(model => model.Password)
            .Length(3, 30)
            .WithMessage("The password must have at least 3 characters and not more than 30");
        RuleFor(model => model.Password)
            .Matches(@"^[a-zA-Z0-9@&#\*]{3,30}$")
            .WithMessage("The password must have at least 3 alphanumeric characters");
    }
}
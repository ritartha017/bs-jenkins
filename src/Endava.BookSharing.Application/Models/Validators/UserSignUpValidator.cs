using Endava.BookSharing.Application.Models.Requests;
using FluentValidation;

namespace Endava.BookSharing.Application.Models.Validators;

public class UserSignUpValidator : AbstractValidator<UserSignUpRequest>
{
    public UserSignUpValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty()
            .WithMessage("The email cannot be empty");
        RuleFor(model => model.Email)
            .Matches("^[\\da-zA-Z.]+@[\\da-zA-Z.]+\\.[\\da-zA-Z.]+$")
            .WithMessage("Invalid email address");

        RuleFor(model => model.FirstName)
            .NotEmpty()
            .WithMessage("The first name cannot be empty");
        RuleFor(model => model.FirstName)
            .Length(3, 30)
            .WithMessage("The first name must have at least 3 characters and not more than 30");
        RuleFor(model => model.FirstName)
            .Matches("^([A-Za-z])*$")
            .WithMessage("The first name must have only latin letters");

        RuleFor(model => model.LastName)
            .NotEmpty()
            .WithMessage("The last name cannot be empty");
        RuleFor(model => model.LastName)
            .Length(3, 30)
            .WithMessage("The last name must have at least 3 characters and not more than 30");
        RuleFor(model => model.LastName)
            .Matches("^([A-Za-z])*$")
            .WithMessage("The last name must have only latin letters");

        RuleFor(model => model.Username)
            .NotEmpty()
            .WithMessage("The username cannot be empty");
        RuleFor(model => model.Username)
            .Length(3, 30)
            .WithMessage("The username must have at least 3 characters and not more than 30");
        RuleFor(model => model.Username)
            .Matches("^[a-zA-Z0-9.$@&*#%]{3,30}$")
            .WithMessage("Invalid username");

        RuleFor(model => model.Password)
            .NotEmpty()
            .WithMessage("The password cannot be empty");
        RuleFor(model => model.Password)
            .Length(3, 30)
            .WithMessage("The password must have at least 3 characters and not more than 30");
        RuleFor(model => model.Password)
            .Matches(@"^[a-zA-Z0-9@&#\*]{3,30}$")
            .WithMessage("Invalid password");
    }
}
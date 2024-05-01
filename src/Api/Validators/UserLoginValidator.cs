using Bll.Models;
using FluentValidation;
using static Dal.Configuration.UserConfiguration;

namespace Api.Validators;

public class UserLoginValidator : AbstractValidator<UserLogin>
{
    private const int PasswordMinLength = 8;
    private const int PasswordMaxLength = 256;

    public UserLoginValidator()
    {
        this.RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MaximumLength(NameMaxLength).WithMessage($"Username must be between 1 and {NameMaxLength} characters.");

        this.RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(PasswordMinLength).WithMessage($"Password must be between {PasswordMinLength} " +
                                                          $"and {PasswordMaxLength} characters.");

    }
}
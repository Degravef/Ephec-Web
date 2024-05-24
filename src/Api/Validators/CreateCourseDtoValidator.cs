using Bll.Models;
using Dal.Configuration;

namespace Api.Validators;

using FluentValidation;

public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseDtoValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(CourseConfiguration.NameMaxLength)
            .WithMessage($"Name must be between 1 and {CourseConfiguration.NameMaxLength} characters.");

        this.RuleFor(x => x.Description)
            .MaximumLength(CourseConfiguration.DescriptionMaxLength)
            .WithMessage($"Description can't be more than {CourseConfiguration.DescriptionMaxLength} characters.");
    }
}

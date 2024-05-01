using Bll.Models;
using FluentValidation;
using static Dal.Configuration.CourseConfiguration;

namespace Api.Validators;

public class UpdateCourseDtoValidator : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseDtoValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(NameMaxLength).WithMessage($"Name must be between 1 and {NameMaxLength} characters.");

        this.RuleFor(x => x.Description)
            .MaximumLength(DescriptionMaxLength).WithMessage($"Description can't be more than {DescriptionMaxLength} characters.");
    }
}
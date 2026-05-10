using Clinic.Api.Models.DTOs;
using FluentValidation;

namespace Clinic.Api.Validators
{
    public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
            RuleFor(x => x.BirthDate).LessThan(DateTime.Now);
        }
    }
}

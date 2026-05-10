using Clinic.Api.Models.DTOs;
using FluentValidation;

namespace Clinic.Api.Validators
{
    public class CreateProfessionalDtoValidator : AbstractValidator<CreateProfessionalDto>
    {
        public CreateProfessionalDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Specialty).NotEmpty().MaximumLength(100);
        }
    }
}

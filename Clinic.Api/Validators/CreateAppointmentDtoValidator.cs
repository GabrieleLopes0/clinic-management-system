using Clinic.Api.Models.DTOs;
using FluentValidation;

namespace Clinic.Api.Validators
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(x => x.PatientId).NotEmpty();
            RuleFor(x => x.ProfessionalId).NotEmpty();
            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.UtcNow.AddMinutes(-1))
                .Must(BeOnThirtyMinuteInterval).WithMessage("Consultas devem iniciar em intervalos de 30 minutos.");
        }

        private static bool BeOnThirtyMinuteInterval(DateTime dateTime)
        {
            return dateTime.Minute % 30 == 0;
        }
    }
}

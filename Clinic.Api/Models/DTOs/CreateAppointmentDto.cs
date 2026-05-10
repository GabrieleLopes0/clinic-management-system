using System;

namespace Clinic.Api.Models.DTOs
{
    public class CreateAppointmentDto
    {
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}

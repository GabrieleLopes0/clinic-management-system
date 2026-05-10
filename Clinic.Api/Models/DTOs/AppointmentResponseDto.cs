using System;

namespace Clinic.Api.Models.DTOs
{
    public class AppointmentResponseDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public Guid ProfessionalId { get; set; }
        public string ProfessionalName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
    }
}

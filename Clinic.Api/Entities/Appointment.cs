using System;

namespace Clinic.Api.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}

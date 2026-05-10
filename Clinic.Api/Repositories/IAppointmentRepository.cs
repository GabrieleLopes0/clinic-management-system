using Clinic.Api.Entities;

namespace Clinic.Api.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetByProfessionalAsync(Guid professionalId);
        Task CreateAsync(Appointment appointment);
        Task<bool> ExistsConflictAsync(Guid professionalId, DateTime appointmentDate);
        Task<bool> ExistsPatientSameProfessionalSameDayAsync(Guid patientId, Guid professionalId, DateTime appointmentDate);
    }
}

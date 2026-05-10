using Clinic.Api.Entities;
using Clinic.Api.Models.DTOs;
using Clinic.Api.Repositories;

namespace Clinic.Api.Services
{
    public interface IAppointmentService
    {
        Task CreateAsync(CreateAppointmentDto request);
        Task<IEnumerable<AppointmentResponseDto>> GetByProfessionalAsync(Guid professionalId);
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IProfessionalRepository _professionalRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IProfessionalRepository professionalRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _professionalRepository = professionalRepository;
        }

        public async Task CreateAsync(CreateAppointmentDto request)
        {
            var patient = await _patientRepository.GetByIdAsync(request.PatientId);
            if (patient == null)
            {
                throw new KeyNotFoundException("Paciente não encontrado.");
            }

            var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId);
            if (professional == null)
            {
                throw new KeyNotFoundException("Profissional não encontrado.");
            }

            ValidateAppointmentDate(request.AppointmentDate);

            if (await _appointmentRepository.ExistsPatientSameProfessionalSameDayAsync(request.PatientId, request.ProfessionalId, request.AppointmentDate))
            {
                throw new InvalidOperationException("Paciente já possui consulta com este profissional no mesmo dia.");
            }

            if (await _appointmentRepository.ExistsConflictAsync(request.ProfessionalId, request.AppointmentDate))
            {
                throw new InvalidOperationException("Profissional já possui consulta neste horário.");
            }

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = request.PatientId,
                ProfessionalId = request.ProfessionalId,
                AppointmentDate = request.AppointmentDate
            };

            await _appointmentRepository.CreateAsync(appointment);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetByProfessionalAsync(Guid professionalId)
        {
            var professional = await _professionalRepository.GetByIdAsync(professionalId);
            if (professional == null)
            {
                throw new KeyNotFoundException("Profissional não encontrado.");
            }

            var appointments = await _appointmentRepository.GetByProfessionalAsync(professionalId);
            var patients = new Dictionary<Guid, string>();

            foreach (var appointment in appointments)
            {
                if (!patients.ContainsKey(appointment.PatientId))
                {
                    var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);
                    patients[appointment.PatientId] = patient?.Name ?? string.Empty;
                }
            }

            return appointments.Select(appointment => new AppointmentResponseDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientName = patients.GetValueOrDefault(appointment.PatientId, string.Empty),
                ProfessionalId = appointment.ProfessionalId,
                ProfessionalName = professional.Name,
                AppointmentDate = appointment.AppointmentDate
            });
        }

        private static void ValidateAppointmentDate(DateTime appointmentDate)
        {
            if (appointmentDate.Kind != DateTimeKind.Utc)
            {
                appointmentDate = appointmentDate.ToUniversalTime();
            }

            var hour = appointmentDate.Hour;
            var dayOfWeek = appointmentDate.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                throw new InvalidOperationException("Consultas só podem ser agendadas de segunda a sexta.");
            }

            if (hour < 8 || hour >= 18)
            {
                throw new InvalidOperationException("O horário deve ser entre 08:00 e 18:00.");
            }

            if (appointmentDate.Minute % 30 != 0)
            {
                throw new InvalidOperationException("Consultas devem iniciar em intervalos de 30 minutos.");
            }
        }
    }
}

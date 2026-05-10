using Clinic.Api.Entities;
using Clinic.Api.Models.DTOs;
using Clinic.Api.Repositories;

namespace Clinic.Api.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponseDto>> GetAllAsync();
        Task<PatientResponseDto?> GetByIdAsync(Guid id);
        Task<PatientResponseDto> CreateAsync(CreatePatientDto request);
        Task UpdateAsync(Guid id, UpdatePatientDto request);
        Task DeleteAsync(Guid id);
    }

    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PatientResponseDto>> GetAllAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return patients.Select(MapToDto);
        }

        public async Task<PatientResponseDto?> GetByIdAsync(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            return patient == null ? null : MapToDto(patient);
        }

        public async Task<PatientResponseDto> CreateAsync(CreatePatientDto request)
        {
            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                BirthDate = request.BirthDate
            };

            await _patientRepository.CreateAsync(patient);
            return MapToDto(patient);
        }

        public async Task UpdateAsync(Guid id, UpdatePatientDto request)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new KeyNotFoundException("Paciente não encontrado.");
            }

            patient.Name = request.Name;
            patient.Email = request.Email;
            patient.BirthDate = request.BirthDate;
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _patientRepository.DeleteAsync(id);
        }

        private static PatientResponseDto MapToDto(Patient patient)
        {
            return new PatientResponseDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Email = patient.Email,
                BirthDate = patient.BirthDate
            };
        }
    }
}

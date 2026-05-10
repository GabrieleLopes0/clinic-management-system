using Clinic.Api.Entities;
using Clinic.Api.Models.DTOs;
using Clinic.Api.Repositories;

namespace Clinic.Api.Services
{
    public interface IProfessionalService
    {
        Task<IEnumerable<ProfessionalResponseDto>> GetAllAsync();
        Task<ProfessionalResponseDto> CreateAsync(CreateProfessionalDto request);
        Task<ProfessionalResponseDto?> GetByIdAsync(Guid id);
    }

    public class ProfessionalService : IProfessionalService
    {
        private readonly IProfessionalRepository _professionalRepository;

        public ProfessionalService(IProfessionalRepository professionalRepository)
        {
            _professionalRepository = professionalRepository;
        }

        public async Task<IEnumerable<ProfessionalResponseDto>> GetAllAsync()
        {
            var professionals = await _professionalRepository.GetAllAsync();
            return professionals.Select(p => new ProfessionalResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Specialty = p.Specialty
            });
        }

        public async Task<ProfessionalResponseDto> CreateAsync(CreateProfessionalDto request)
        {
            var professional = new Professional
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Specialty = request.Specialty
            };

            await _professionalRepository.CreateAsync(professional);
            return new ProfessionalResponseDto
            {
                Id = professional.Id,
                Name = professional.Name,
                Specialty = professional.Specialty
            };
        }

        public async Task<ProfessionalResponseDto?> GetByIdAsync(Guid id)
        {
            var professional = await _professionalRepository.GetByIdAsync(id);
            if (professional == null)
            {
                return null;
            }

            return new ProfessionalResponseDto
            {
                Id = professional.Id,
                Name = professional.Name,
                Specialty = professional.Specialty
            };
        }
    }
}

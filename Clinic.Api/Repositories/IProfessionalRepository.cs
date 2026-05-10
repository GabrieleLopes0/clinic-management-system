using Clinic.Api.Entities;

namespace Clinic.Api.Repositories
{
    public interface IProfessionalRepository
    {
        Task<IEnumerable<Professional>> GetAllAsync();
        Task<Professional?> GetByIdAsync(Guid id);
        Task CreateAsync(Professional professional);
    }
}

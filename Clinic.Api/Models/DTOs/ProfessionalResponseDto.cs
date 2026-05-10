using System;

namespace Clinic.Api.Models.DTOs
{
    public class ProfessionalResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
    }
}

using System;

namespace Clinic.Api.Models.DTOs
{
    public class UpdatePatientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}

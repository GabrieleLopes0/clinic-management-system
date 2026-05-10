using System;

namespace Clinic.Api.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}

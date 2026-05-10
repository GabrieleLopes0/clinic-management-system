using System;

namespace Clinic.Api.Entities
{
    public class Professional
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
    }
}

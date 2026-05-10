using Clinic.Api.Entities;
using Clinic.Api.Models.DTOs;
using Clinic.Api.Repositories;
using Clinic.Api.Services;

namespace Clinic.Tests;

public class AppointmentServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenAppointmentOnWeekend()
    {
        var service = BuildService();
        var request = new CreateAppointmentDto
        {
            PatientId = Guid.NewGuid(),
            ProfessionalId = Guid.NewGuid(),
            AppointmentDate = new DateTime(2026, 5, 10, 10, 0, 0, DateTimeKind.Utc) // Saturday
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenAppointmentOutsideBusinessHours()
    {
        var service = BuildService();
        var request = new CreateAppointmentDto
        {
            PatientId = Guid.NewGuid(),
            ProfessionalId = Guid.NewGuid(),
            AppointmentDate = new DateTime(2026, 5, 9, 7, 30, 0, DateTimeKind.Utc) // before 08:00
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenPatientHasSameProfessionalSameDay()
    {
        var patientId = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var appointmentDate = new DateTime(2026, 5, 12, 10, 0, 0, DateTimeKind.Utc);

        var appointmentRepository = new FakeAppointmentRepository
        {
            ExistingSamePatientProfessionalSameDay = true
        };

        var service = BuildService(appointmentRepository);
        var request = new CreateAppointmentDto
        {
            PatientId = patientId,
            ProfessionalId = professionalId,
            AppointmentDate = appointmentDate
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenProfessionalHasConflictAtSameTime()
    {
        var patientId = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var appointmentDate = new DateTime(2026, 5, 12, 10, 0, 0, DateTimeKind.Utc);

        var appointmentRepository = new FakeAppointmentRepository
        {
            ExistingProfessionalConflict = true
        };

        var service = BuildService(appointmentRepository);
        var request = new CreateAppointmentDto
        {
            PatientId = patientId,
            ProfessionalId = professionalId,
            AppointmentDate = appointmentDate
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));
    }

    private static AppointmentService BuildService(FakeAppointmentRepository? appointmentRepository = null)
    {
        var patientRepository = new FakePatientRepository();
        var professionalRepository = new FakeProfessionalRepository();
        return new AppointmentService(
            appointmentRepository ?? new FakeAppointmentRepository(),
            patientRepository,
            professionalRepository);
    }

    private class FakePatientRepository : IPatientRepository
    {
        public Task CreateAsync(Patient patient) => Task.CompletedTask;
        public Task DeleteAsync(Guid id) => Task.CompletedTask;
        public Task<IEnumerable<Patient>> GetAllAsync() => Task.FromResult(Enumerable.Empty<Patient>());
        public Task<Patient?> GetByIdAsync(Guid id) => Task.FromResult<Patient?>(new Patient { Id = id, Name = "Teste", Email = "teste@clinic.com", BirthDate = DateTime.UtcNow.AddYears(-20) });
        public Task UpdateAsync(Patient patient) => Task.CompletedTask;
    }

    private class FakeProfessionalRepository : IProfessionalRepository
    {
        public Task CreateAsync(Professional professional) => Task.CompletedTask;
        public Task<IEnumerable<Professional>> GetAllAsync() => Task.FromResult(Enumerable.Empty<Professional>());
        public Task<Professional?> GetByIdAsync(Guid id) => Task.FromResult<Professional?>(new Professional { Id = id, Name = "Dr. Teste", Specialty = "Geral" });
    }

    private class FakeAppointmentRepository : IAppointmentRepository
    {
        public bool ExistingProfessionalConflict { get; set; }
        public bool ExistingSamePatientProfessionalSameDay { get; set; }

        public Task CreateAsync(Appointment appointment) => Task.CompletedTask;
        public Task<bool> ExistsConflictAsync(Guid professionalId, DateTime appointmentDate) => Task.FromResult(ExistingProfessionalConflict);
        public Task<bool> ExistsPatientSameProfessionalSameDayAsync(Guid patientId, Guid professionalId, DateTime appointmentDate) => Task.FromResult(ExistingSamePatientProfessionalSameDay);
        public Task<IEnumerable<Appointment>> GetByProfessionalAsync(Guid professionalId) => Task.FromResult(Enumerable.Empty<Appointment>());
    }
}

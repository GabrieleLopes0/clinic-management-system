using Clinic.Api.Data;
using Clinic.Api.Entities;
using Dapper;

namespace Clinic.Api.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AppointmentRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Appointment>> GetByProfessionalAsync(Guid professionalId)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM appointments WHERE professionalid = @ProfessionalId ORDER BY appointmentdate";
            return await connection.QueryAsync<Appointment>(sql, new { ProfessionalId = professionalId });
        }

        public async Task CreateAsync(Appointment appointment)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"INSERT INTO appointments (id, patientid, professionalid, appointmentdate)
VALUES (@Id, @PatientId, @ProfessionalId, @AppointmentDate)";
            await connection.ExecuteAsync(sql, appointment);
        }

        public async Task<bool> ExistsConflictAsync(Guid professionalId, DateTime appointmentDate)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"SELECT COUNT(1) FROM appointments WHERE professionalid = @ProfessionalId AND appointmentdate = @AppointmentDate";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { ProfessionalId = professionalId, AppointmentDate = appointmentDate });
            return count > 0;
        }

        public async Task<bool> ExistsPatientSameProfessionalSameDayAsync(Guid patientId, Guid professionalId, DateTime appointmentDate)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"SELECT COUNT(1) FROM appointments
WHERE patientid = @PatientId
AND professionalid = @ProfessionalId
AND DATE(appointmentdate) = DATE(@AppointmentDate)";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { PatientId = patientId, ProfessionalId = professionalId, AppointmentDate = appointmentDate });
            return count > 0;
        }
    }
}

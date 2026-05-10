using Clinic.Api.Data;
using Clinic.Api.Entities;
using Dapper;

namespace Clinic.Api.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PatientRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM patients ORDER BY name";
            return await connection.QueryAsync<Patient>(sql);
        }

        public async Task<Patient?> GetByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM patients WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Patient>(sql, new { Id = id });
        }

        public async Task CreateAsync(Patient patient)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"INSERT INTO patients (id, name, email, birthdate)
VALUES (@Id, @Name, @Email, @BirthDate)";
            await connection.ExecuteAsync(sql, patient);
        }

        public async Task UpdateAsync(Patient patient)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"UPDATE patients SET name = @Name, email = @Email, birthdate = @BirthDate WHERE id = @Id";
            await connection.ExecuteAsync(sql, patient);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "DELETE FROM patients WHERE id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

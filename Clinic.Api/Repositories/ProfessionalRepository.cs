using Clinic.Api.Data;
using Clinic.Api.Entities;
using Dapper;

namespace Clinic.Api.Repositories
{
    public class ProfessionalRepository : IProfessionalRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ProfessionalRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Professional>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM professionals ORDER BY name";
            return await connection.QueryAsync<Professional>(sql);
        }

        public async Task<Professional?> GetByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM professionals WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Professional>(sql, new { Id = id });
        }

        public async Task CreateAsync(Professional professional)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"INSERT INTO professionals (id, name, specialty)
VALUES (@Id, @Name, @Specialty)";
            await connection.ExecuteAsync(sql, professional);
        }
    }
}

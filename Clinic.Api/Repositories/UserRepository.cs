using Clinic.Api.Data;
using Clinic.Api.Entities;
using Dapper;
using System.Data;

namespace Clinic.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM users WHERE email = @Email";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task CreateAsync(User user)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"INSERT INTO users (id, email, passwordhash, role)
VALUES (@Id, @Email, @PasswordHash, @Role)";
            await connection.ExecuteAsync(sql, user);
        }
    }
}

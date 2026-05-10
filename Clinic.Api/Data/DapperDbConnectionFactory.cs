using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Clinic.Api.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class DapperDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DapperDbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new NpgsqlConnection(connectionString);
        }
    }
}

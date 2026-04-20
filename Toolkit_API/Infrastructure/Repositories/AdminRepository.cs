using Toolkit_API.Application.Interfaces;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Toolkit_API.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepo
    {
        public async Task<IEnumerable<string>> GetAllUsers()
        {
            var sqlQuery = "SELECT id,username,newemail FROM Users";
            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DefaultConnection")))
            {
                var users = await connection.QueryAsync<string>(sqlQuery);
                return users;
            }
        }
    }
}

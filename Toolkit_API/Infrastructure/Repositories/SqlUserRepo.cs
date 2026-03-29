using Dapper;
using Microsoft.Data.SqlClient;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Users;

namespace Toolkit_API.Infrastructure.Repositories
{
    public class SqlUserRepo : IUserRepo
    {
        private readonly string _connectionString;
        private readonly IPasswordHasher _passwordHasher;
        public SqlUserRepo(IPasswordHasher passwordHasher, string connectionString)
        {
            _passwordHasher = passwordHasher;
            _connectionString = connectionString;
        }
        public async Task<Users> GetUser(string username)
        {
            string sqlQuery = "Select * From Users Where username = @Username";

            using (var conn = new SqlConnection(_connectionString))
            {
                var user = await conn.QuerySingleAsync<Users>(sqlQuery, new { Username = username });
                return user;
            }
        }
        public async Task<Users> Login(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            string sqlQuery = "Select * From Users where username = @Username and passwordHash = @PasswordHash and passwordSalt = @PasswordSalt";

            using (var conn = new SqlConnection(_connectionString))
            {
                var response = await conn.QueryFirstOrDefaultAsync<Users>(sqlQuery, new
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt

                });
                return response;
            }
        }

        public async Task<Users> CreateUser(string username, string email, byte[] passwordHash, byte[] passwordSalt)
        {
            // Note to me : Output Inserted.* is used to return the inserted record after the insert operation is performed.

            string sqlQuery = "Insert Into Users (username,passwordHash,passwordSalt,newemail) OUTPUT INSERTED.* values (@Username,@PasswordHash,@PasswordSalt,@Email)";

            using (var conn = new SqlConnection(_connectionString))
            {
                var response = await conn.QuerySingleAsync<Users>(sqlQuery, new
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = email,

                });

                return response;
            }

<<<<<<< HEAD
=======
                    return response;
                }
               
                
>>>>>>> 246ee978b81f42224a20454d8892c639a8ea1bb5
        }
        public async Task<bool> UserExists(string username)
        {
            string sqlQuery = "Select Count(1) From Users where username = @Username";
            using (var conn = new SqlConnection(_connectionString))
            {
                var response = await conn.QueryFirstAsync<bool>(sqlQuery, new { Username = username });
                return response;
            }
        }
        public async Task<string> TestConnection()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var response = await conn.QueryAsync("Select * from Users");

                return response.ToString();
            }
        }

    }
}

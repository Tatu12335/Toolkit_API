using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Linq.Expressions;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Users;
using Toolkit_API.Middleware.Exceptions;

namespace Toolkit_API.Infrastructure.Repositories
{
    public class SqlUserRepo : IUserRepo 
    {
        private readonly string _connectionString;
        private readonly IPasswordHasher _passwordHasher;
        public SqlUserRepo(IPasswordHasher passwordHasher,string connectionString)
        {
            _passwordHasher = passwordHasher;
            _connectionString = connectionString;
        }
        public async Task<Users> GetUser(string username)
        {
            string sqlQuery = "Select * From Users Where username = @Username";
            
                using (var conn = new SqlConnection(_connectionString))
                {
                    var user = await conn.QueryFirstOrDefaultAsync<Users>(sqlQuery, new {Username = username});
                    return user;
                }
            
            

        }
        public async Task<Users> Login(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            string sqlQuery = "Select Count(1) From Users where (username = @Username and passwordHash = @PasswordHash and passwordSalt = @PasswordSalt)";

            using(var conn = new SqlConnection(_connectionString))
            {
                var response = await conn.QueryFirstAsync<Users>(sqlQuery, new 
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                
                });
                return response;
            }
                     
        }
        public async Task <Users> CreateUser(string username,string email, byte[] passwordHash, byte[] passwordSalt)
        {

            string sqlQuery = "Insert Into Users (username,passwordHash,passwordSalt,newemail) values (@Username,@PasswordSalt,@PasswordHash,@Email)";
           
                using (var conn = new SqlConnection(_connectionString))
                {
                    var response = await conn.ExecuteScalarAsync<Users>(sqlQuery, new
                    {
                        Username = username,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Email = email,

                    });

                    return response;
                }
               
                
        }
        public async Task <string> TestConnection()
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                var response = await conn.QueryAsync("Select * from Users");

                return response.ToString();
            }
        }

    }
}

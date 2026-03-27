using Dapper;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Users;

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
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var user = await conn.QueryFirstOrDefaultAsync<Users>(sqlQuery, new {Username = username});

                    if (user != null) return user;

                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }

        }
        public async Task <UserSession> CreateUser(string username,string email, string password)
        {

            string sqlQuery = "Insert Into Users (username,passwordSalt,passwordHash,email) values (@Username,@PasswordSalt,@PasswordHash,@Email)";

            byte[] passwordSalt;

            var passwordHash = _passwordHasher.HashPassword(password, out passwordSalt);

            //var users = await GetUser(username);

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var response = await conn.ExecuteAsync(sqlQuery, new
                    {
                        Username = username,
                        Email = email,
                        PasswordSalt = passwordSalt,
                        PasswordHash = passwordHash

                    });

                    if(response != 0 || response != null) return new UserSession(username);
                }

                

            }
            catch (Exception ex)
            {
                return null;
            }
            
           

            return null;
        }
    }
}

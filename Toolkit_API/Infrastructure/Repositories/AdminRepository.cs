using Dapper;
using Microsoft.Data.SqlClient;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Users;

namespace Toolkit_API.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepo
    {
        private readonly string _connectionString;

        public AdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ForAdminEntity>> GetAllUsers()
        {
            var sqlQuery = "SELECT id,username,newemail,roles FROM Users";
            using (var connection = new SqlConnection(_connectionString))
            {
                var users = await connection.QueryAsync<ForAdminEntity>(sqlQuery);

                return users;
            }
        }
        public async Task<string> CheckAdminStatus(int userId)
        {
            var sqlQuery = "SELECT roles FROM Users WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var role = await connection.QueryFirstOrDefaultAsync<string>(sqlQuery, new { UserId = userId });
                return role;
            }
        }
        public async Task<bool> CheckUserExists(int userId)
        {
            var sqlQuery = "SELECT COUNT(1) FROM Users WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var exists = await connection.QueryFirstAsync<bool>(sqlQuery, new { UserId = userId });
                return exists;
            }
        }
        public async Task<string> GetUserEmail(int userId)
        {
            var sqlQuery = "SELECT newemail FROM Users WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var email = await connection.QueryFirstOrDefaultAsync<string>(sqlQuery, new { UserId = userId });
                return email;
            }
        }
        public async Task<string> GetUsername(int userId)
        {
            var sqlQuery = "SELECT username FROM Users WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var username = await connection.QueryFirstOrDefaultAsync<string>(sqlQuery, new { UserId = userId });
                return username;
            }
        }
        public async Task<string> GetUserRole(int userId)
        {
            var sqlQuery = "SELECT roles FROM Users WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var role = await connection.QueryFirstOrDefaultAsync<string>(sqlQuery, new { UserId = userId });
                return role;
            }
        }
        public async Task<int> DeleteUser(int userId)
        {
            var sqlQuery = "DELETE FROM Users WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = await connection.ExecuteAsync(sqlQuery, new { UserId = userId });
                return affectedRows;
            }
        }
        public async Task<int> UpdateUserRole(int userId, string newRole)
        {
            var sqlQuery = "UPDATE Users SET roles = @NewRole WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = await connection.ExecuteAsync(sqlQuery, new { UserId = userId, NewRole = newRole });
                return affectedRows;
            }
        }
        public async Task<int> UpdateUserEmail(int userId, string newEmail)
        {
            var sqlQuery = "UPDATE Users SET newemail = @NewEmail WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = await connection.ExecuteAsync(sqlQuery, new { UserId = userId, NewEmail = newEmail });
                return affectedRows;
            }
        }
        public async Task<int> UpdateUsername(int userId, string newUsername)
        {
            var sqlQuery = "UPDATE Users SET username = @NewUsername WHERE id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = await connection.ExecuteAsync(sqlQuery, new { UserId = userId, NewUsername = newUsername });
                return affectedRows;
            }
        }
        public async Task<ForAdminEntity> SearchUserByName(string username)
        {
            var sqlQuery = "SELECT id,username,newemail FROM Users WHERE username LIKE @Username";
            using (var connection = new SqlConnection(_connectionString))
            {
                var user = await connection.QueryFirstOrDefaultAsync<ForAdminEntity>(sqlQuery, new { Username = username });
                return user;
            }
        }
        public async Task<int> DeleteUserByName(string username)
        {
            var sqlQuery = "DELETE FROM Users WHERE username = @Username";
            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = await connection.ExecuteAsync(sqlQuery, new { Username = username });
                return affectedRows;
            }
        }
        public async Task<bool> SearchActiveUsers(string username)
        {
            var sqlQuery = "SELECT COUNT(1) FROM Users WHERE username LIKE @Username AND isActive = 1";
            using (var connection = new SqlConnection(_connectionString))
            {
                var exists = await connection.QueryFirstAsync<bool>(sqlQuery, new { Username = username });
                return exists;
            }
        }
    }
}

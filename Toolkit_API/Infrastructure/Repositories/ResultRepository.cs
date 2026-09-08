using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Middleware;

namespace Toolkit_API.Infrastructure.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly string _connectionString;

        public ResultRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task <ScanResult> GetCapabilities(string jobId)
        {
            string query = "SELECT JsonData FROM ScanResult WHERE jobId = @JobId";

            using (var conn = new SqlConnection(_connectionString))
            {
                var sqlResult = await conn.QuerySingleAsync<ScanResult>(query, new { JobId = jobId });
                //var final = sqlResult.capabilities.Select(x => x.ToString()).ToList();
                return sqlResult;
            }
        }
        public async Task <ScanResult> GetResultAsync(string jobId)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                Debug.WriteLine($"Getting result for jobId: {jobId}");
                string query = "SELECT JsonData FROM ScanResult WHERE jobId = @JobId";
                string jsonData = await connection.QuerySingleAsync<string>(query, new { JobId = jobId });

                var json = JsonSerializer.Deserialize<ScanResult>(jsonData);               

                return json;


            }

        }
        public async Task SaveResultAsync(string jobId, ScanResult result)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                
                var JsonData = JsonSerializer.Serialize(result);
                    
                Debug.WriteLine($"Saving result for jobId: {jobId}, jsonData: {JsonData}");

                string query = "INSERT INTO ScanResult (jobId, JsonData) VALUES (@JobId, @JsonData)";
                await connection.ExecuteAsync(query, new { JobId = jobId, JsonData = JsonData });

                
            }
        }
    }
}

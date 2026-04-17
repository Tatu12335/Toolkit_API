using Dapper;
using Microsoft.Data.SqlClient;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Infrastructure.Services;
using Toolkit_API.Domain.Entities.FileAnalysis;
using Toolkit_API.Domain.Entities.Files;
using System.Runtime.InteropServices;

namespace Toolkit_API.Infrastructure.Repositories
{
    public class FileScanRepo : IFileScanRepo
    {
        private readonly FileHasher _hasher;
        private readonly string _connetionString;
        public FileScanRepo(FileHasher hasher, string connetionString)
        {
            _hasher = hasher;
            _connetionString = connetionString;
        }
        public async Task<int> InsertHash(string filePath, int userId, byte[] hash)
        {

            
            var fileInfo = new FileInfo(filePath);


            using (var conn = new SqlConnection(_connetionString))
            {
                var fileId = await conn.QuerySingleAsync<int>("Insert Into ScanLog (FileName, FileHash, userId) OUTPUT INSERTED.id values (@FileName, @FileHash, @UserId)", new
                {
                    FileName = fileInfo.Name,
                    FileHash = hash,
                    UserId = userId,

                });
                return fileId;
            }
        }
        public async Task<byte[]> InsertAll(string filePath, int userId,double score)
        {
            var hash = await _hasher.HashFileAsync(filePath);
            var fileId = await InsertHash(filePath, userId, hash); 
            await InsertScore(fileId, score);
            return hash;
        }
        public async Task<FileScanLog> GetScanLog(int logId)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                var log = await conn.QueryFirstOrDefaultAsync<FileScanLog>("Select * From ScanLog Where Id = @Id", new { Id = logId });
                return log;
            }
        }
        public async Task InsertScore(int logId,double score)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                
                await conn.ExecuteAsync("Update ScanLog Set score = @Score Where id = @Id", new { Score = score, Id = logId });
                
            }
        }
    }
}

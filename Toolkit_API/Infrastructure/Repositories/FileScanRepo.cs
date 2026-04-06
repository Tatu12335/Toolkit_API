using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Infrastructure.Services;

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
        public async Task ScanFile(string filePath, int userId)
        {
            
            var hash = await _hasher.HashFileAsync(filePath);
            var fileInfo = new FileInfo(filePath);

            
            using (var conn = new SqlConnection(_connetionString))
            {
                var file = await conn.ExecuteAsync("Insert Into ScanLog (FileName, FileHash, userId,detectionStatus) values (@FileName, @FileHash, @UserId)", new
                {
                    FileName = fileInfo.Name,
                    FileHash = hash,
                    UserId = userId,
                });
           

            }
        }
    }
}

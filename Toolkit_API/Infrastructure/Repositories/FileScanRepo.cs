using Dapper;
using Microsoft.Data.SqlClient;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Domain.Policies;
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
        public async Task<FileScanLog> GetFile(byte[] hash, int userId)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                var file = await conn.QueryFirstOrDefaultAsync<FileScanLog>("Select * From ScanLog Where FileHash = @Hash And userId = @UserId", new { Hash = hash, UserId = userId });
                return file;
            }
        }
        public async Task<byte[]> InsertAll(string filePath, int userId, double score)
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
        public async Task InsertScore(int logId, double score)
        {
            using (var conn = new SqlConnection(_connetionString))
            {

                await conn.ExecuteAsync("Update ScanLog Set score = @Score Where id = @Id", new { Score = score, Id = logId });

            }
        }
        public async Task<IEnumerable<int>> GetFileId(byte[] FileHash, int userId)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                var result = await conn.QueryAsync<int>("SELECT id FROM ScanLog where FileHash = @FileHash", new { Filehash = FileHash });
                return result;
            }
        }
        public async Task<byte[]> DoubleHash(byte[] hash)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                var existingHash = await conn.QueryFirstOrDefaultAsync<byte[]>("Select FileHash From ScanLog Where FileHash = @Hash", new { Hash = hash });
                return existingHash;
            }
        }
        public async Task InsertCapabalities(byte[] FileHash, int userId, IEnumerable<Capability> capabilities)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                foreach (var capability in capabilities)
                {
                    await conn.ExecuteAsync("Update ScanLog Set capabilities = @Capabilities Where FileHash = @FileHash and userId = @UserId",
                        new { FileHash = FileHash, UserId = userId, Capabilities = capability });
                }
            }
        }
        public async Task<IEnumerable<Capability>> GetCapability(byte[] FileHash, int userId)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                var capability = await conn.QueryAsync<Capability>("Select capabilities FROM ScanLog Where FileHash = @FileHash and userId = @UserId",
                new { FileHash = FileHash, UserId = userId });

                return capability;
            }
        }
        public async Task InsertScanResult(string fileName,byte[] FileHash, int userId, double score, IEnumerable<Capability> capabilities, double confidence, double severity)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                await conn.ExecuteAsync("Insert Into ScanLog (FileName,FileHash, userId, score, capabilities, confidence, severity) values (@FileName, @FileHash, @UserId, @Score, @Capabilities, @Confidence, @Severity)",
                    new { FileName = fileName, FileHash = FileHash, UserId = userId, Score = score, Capabilities = capabilities, Confidence = confidence, Severity = severity });
            }
        }
        public async Task InsertScanLogObject(FileScanLog fileScanLog)
        {
            using (var conn = new SqlConnection(_connetionString))
            {
                await conn.ExecuteAsync("Insert Into ScanLog (FileName, FileHash, userId, Score, capabilities, confidence, severity) values (@FileName, @FileHash, @UserId, @Score, @Capabilities, @Confidence, @Severity)", new
                {
                    FileName = fileScanLog.FileName,    
                    FileHash = fileScanLog.FileHash,
                    UserId = fileScanLog.userId,
                    Score = fileScanLog.Score,
                    Capabilities = fileScanLog.Capability,
                    Confidence = fileScanLog.confidence,
                    Severity = fileScanLog.severity
                });
            }
        }
    }
}

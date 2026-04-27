using Toolkit_API.Domain.Entities.Files;

namespace Toolkit_API.Application.Interfaces
{
    public interface IFileScanRepo
    {
        public Task<FileScanLog> GetScanLog(int logId);
        public Task InsertScore(int logId, double score);
        public Task<byte[]> InsertAll(string filePath, int userId, double score);
        public Task<IEnumerable<byte[]>> DoubleHash(byte[] hash);
        public Task<FileScanLog> GetFile(byte[] hash, int userId);
    }
}

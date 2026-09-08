using Toolkit_API.Domain.Entities.Files;

namespace Toolkit_API.Application.Interfaces
{
    public interface IResultRepository
    {
        public Task<ScanResult> GetResultAsync(string jobId);
        public Task SaveResultAsync(string jobId, ScanResult result);
        public Task<ScanResult> GetCapabilities(string jobId);
    }
}

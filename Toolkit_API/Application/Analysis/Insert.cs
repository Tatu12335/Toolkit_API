using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Domain.Policies;

namespace Toolkit_API.Application.Analysis
{
    public class Insert
    {
        private readonly IFileScanRepo _fileScanRepository;

        public Insert(IFileScanRepo fileScanRepository)
        {
            _fileScanRepository = fileScanRepository;
        }

        public async Task InsertFile(string fileName, byte[] FileHash, int userId, double score, IEnumerable<Capability> capabilities, double confidence, double severity)
        {
            if(capabilities == null || !capabilities.Any())
            {
                capabilities = new List<Capability> { Capability.None };
            }
            if(score < 0 || score > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100.");
            }
            if(confidence < 0 || confidence > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(confidence), "Confidence must be between 0 and 100.");
            }
            if(severity < 0 || severity > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(severity), "Severity must be between 0 and 100.");
            }
            

            await _fileScanRepository.InsertScanResult(fileName, FileHash, userId, score, capabilities, confidence, severity);
        }
    }
}

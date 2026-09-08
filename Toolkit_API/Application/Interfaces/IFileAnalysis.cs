using Toolkit_API.Domain.Entities.FileAnalysis;
using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Domain.Policies;
namespace Toolkit_API.Application.Interfaces
{
    public interface IFileAnalysis
    {
        public Task<string> Detect(byte[] bytes);
        public Task<string> AnalyzeFile(string filePath);
        public Task ExtensionMatches(string filepath);
        public IEnumerable<Capability> FindDetections(byte[] bytes, ExtractedStrings extractedStrings);
        public Task<IEnumerable<Capability>> ComboDetection(string filePath, ExtractedStrings extractedStrings);
        public Task <IEnumerable<Capability>> ImportAnalysis(string filePath, ExtractedStrings extractedStrings);

    }
}

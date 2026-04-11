using Toolkit_API.Domain.Entities.FileAnalysis;
namespace Toolkit_API.Application.Interfaces
{
    public interface IFileAnalysis
    {
        public Task<string> Detect(byte[] bytes);
        public byte[] DetermineMagicBytes(byte[] bytes);
        public Task<string> AnalyzeFile(string filePath);
        public Task<bool> ExtensionMatches(string filepath);
        public Task<bool> CheckForSuspiciousPatterns(string filePath, FileAnalysisResult fileAnalysisResult, ExtractedStrings extractedStrings);
    }
}

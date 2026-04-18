using System.Diagnostics;
using Toolkit_API.Application.Analysis;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.FileAnalysis;

namespace Toolkit_API.Application.Application_Services.Operations
{
    public class StaticFileAnalysis
    {
        private readonly IFileAnalysis _fileAnalysis;
        private readonly ScoringAlg _scoringAlg;
        private readonly ExtractedStrings _extractedStrings;

        public StaticFileAnalysis(IFileAnalysis fileAnalysis, ScoringAlg scoringAlg, ExtractedStrings extractedStrings)
        {
            _fileAnalysis = fileAnalysis;
            _scoringAlg = scoringAlg;
            _extractedStrings = extractedStrings;
        }
        public async Task<FileAnalysisResult> AnalyzeFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            var analysisResult = await _fileAnalysis.AnalyzeFile(filePath);
            var extensionMatch = await _fileAnalysis.ExtensionMatches(filePath);
            var metadataBool = await _fileAnalysis.CheckForSuspiciousPatterns(filePath, _extractedStrings);

            var score = await _scoringAlg.CalculateScore(filePath, metadataBool, extensionMatch);


            Debug.WriteLine($"File Analysis Result: {analysisResult}");
            Debug.WriteLine($"File Analysis Score: {(double)score}");
            return new FileAnalysisResult
            {
                FilePath = filePath,
                AnalysisResult = analysisResult,
                Score = score
            };
        }
    }
}

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
       
        public StaticFileAnalysis(IFileAnalysis fileAnalysis, ScoringAlg scoringAlg) 
        { 
            _fileAnalysis = fileAnalysis;
            _scoringAlg = scoringAlg;
        }
        public async Task<FileAnalysisResult> AnalyzeFileExtension(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if (!File.Exists(filePath))
                throw new FileNotFoundException();
            

            var extension = Path.GetExtension(filePath);

            var analysisResult = await _fileAnalysis.AnalyzeFile(filePath);

            var fileAnalysisResult = new FileAnalysisResult
            {
                FilePath = filePath,
                AnalysisResult = analysisResult
            };

            var score = await _scoringAlg.CalculateScore(filePath, fileAnalysisResult);

            

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

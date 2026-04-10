using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.FileAnalysis;

namespace Toolkit_API.Application.Analysis
{
    public class ScoringAlg
    {
        private const int MaxScore = 100;
        private const int MinScore = 0;

        private readonly IFileAnalysis _fileAnalysis;
        public readonly HandleResult _handleResult;
        private double _score;
        public ScoringAlg(IFileAnalysis fileAnalysis, HandleResult handleResult, double score)
        {
            _fileAnalysis = fileAnalysis;
            _handleResult = handleResult;
            this._score = score;
        }

        public async Task <double> CalculateScore(string filepath,FileAnalysisResult fileAnalysisResult)
        {

            var extensionMatches = await _fileAnalysis.ExtensionMatches(filepath);
            var suspiciousPatterns = await _fileAnalysis.CheckForSuspiciousPatterns(filepath, fileAnalysisResult, new ExtractedStrings());
            


            if (suspiciousPatterns)
                _score += 30.0; // Penalty for suspicious patterns

            if (!extensionMatches)
                _score += 20.0; // Penalty for extension mismatch
            
            switch (_score)
            {
                case > MaxScore:
                    _score = MaxScore;
                    break;
                
                case < MinScore:
                    _score = MinScore;
                    break;
            }


            return _score;
        }
    }
}

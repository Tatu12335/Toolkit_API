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
        private readonly ExtractedStrings _extractedStrings;
        public ScoringAlg(IFileAnalysis fileAnalysis, HandleResult handleResult, double score, ExtractedStrings extractedStrings)
        {
            _fileAnalysis = fileAnalysis;
            _handleResult = handleResult;
            this._score = score;
            _extractedStrings = extractedStrings;
        }

        public async Task<double> CalculateScore(string filepath, bool suspiciousPatterns, bool extensionMatches)
        {
            if (suspiciousPatterns)
                _score += 30.0; // Penalty for suspicious patterns

            if (!extensionMatches)
                _score += 20.0; // Penalty for extension mismatch
            
            switch(_score)
            {
                case >= 80.0:
                    _score += 10.0; // Bonus for highly suspicious files
                    break;
                case >= 50.0:
                    _score += 5.0; // Bonus for moderately suspicious files
                    break;
                case >= 20.0:
                    _score += 2.0; // Bonus for slightly suspicious files
                    break;
            }
            if(_score > MaxScore)
                _score = MaxScore;
            if(_score < MinScore)
                _score = MinScore;
           

            return _score;
        }
    }
}

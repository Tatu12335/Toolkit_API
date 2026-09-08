using MimeKit.Cryptography;
using System.Diagnostics;
using Toolkit_API.Application.Application_Services.FileOperations;
using Toolkit_API.Application.Calculators;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.FileAnalysis;
using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Domain.Policies;
using Toolkit_API.Middleware;

namespace Toolkit_API.Application.Analysis
{
    public class StaticScan
    {
        // Fix this dependency jungle !!!!!!!!
        private readonly IFileScanRepo _fileScanRepository;
        private readonly HashOps _hashOps;
        private readonly ICallExternalAPI _callExternalAPI;
        private readonly Calculate_Risk_Level _Risk_Level;
        private readonly ICapabilityAnalyzer _capabilityAnalyzer;
        private readonly ExtractedStrings _extractedStrings;
        private readonly IFileAnalysis _fileAnalysis;
        private readonly IResultRepository _resultRepo;
        private readonly IDetectionSourceBuilder _detectionSourceBuilder;
        private readonly ConfidenceANDSeverityCalculator _confidenceANDSeverityCalculator;
        private readonly ScoringAlgorithmn _scoringAlgorithmn;
        private readonly Insert _insert;
        public StaticScan(IFileScanRepo fileScanRepository,
            HashOps hashOps,
            ICallExternalAPI callExternalAPI,
            Calculate_Risk_Level risk_Level,
            ICapabilityAnalyzer capabilityAnalyzer,
            ExtractedStrings extractedStrings,
            IFileAnalysis fileAnalysis,
            IResultRepository resultRepository,
            IDetectionSourceBuilder detectionSourceBuilder,
            ConfidenceANDSeverityCalculator confidenceANDSeverityCalculator,
            ScoringAlgorithmn scoringAlgorithmn,
            Insert insert)    
        {
            _fileScanRepository = fileScanRepository;
            _hashOps = hashOps;
            _callExternalAPI = callExternalAPI;
            _capabilityAnalyzer = capabilityAnalyzer;
            _insert = insert;
            _Risk_Level = risk_Level;
            _extractedStrings = extractedStrings;
            _fileAnalysis = fileAnalysis;
            _resultRepo = resultRepository;
            _detectionSourceBuilder = detectionSourceBuilder;
            _confidenceANDSeverityCalculator = confidenceANDSeverityCalculator;
            _scoringAlgorithmn = scoringAlgorithmn;
        }
        public async Task<ScanResult> ScanFile(string filepath, int userId)
        {
            Debug.WriteLine($"Scanning file: {filepath} for user: {userId}");
            if (string.IsNullOrWhiteSpace(filepath))
                return null;

            var capabilities = new List<Capability>();
            filepath = Path.GetFullPath(filepath);

            Debug.WriteLine($"Full path of the file: {filepath}");


            var file = await _hashOps.ComputeFileHashAsync(filepath, userId);
            //Debug.WriteLine($"File hash: {BitConverter.ToString(File.FileHash).Replace("-", "").ToLower()}");
            


            //var MalwareBazaarResult = await _callExternalAPI.CallAPI(File.FileHash, Environment.GetEnvironmentVariable("Malware_Bazaar_key"));
            var Patterns = await _fileAnalysis.ComboDetection(filepath, _extractedStrings);
            var DetectionSource = await _detectionSourceBuilder.CreateContext(filepath, _extractedStrings);
            
            if(!DetectionSource.Any())
                return new ScanResult
                { 
                    score = 0,
                    fileHash = file.FileHash,
                    fileName = file.FileName,
                    Sources = DetectionSource,
                    severity = 0,
                    confidence = 0
                };


            Debug.WriteLine($"Patterns found: {Patterns?.Count() ?? 0}");

            // you might be thinking, was this really necessary? yes, it was.
            // I want to make sure that the patterns are not null and that they contain at least one element before proceeding with the loop.
            // This is a defensive programming practice to avoid potential null reference exceptions or unnecessary iterations over an empty collection.
            if (Patterns != null && Patterns.Any())
            {
                foreach (var pattern in Patterns)
                {
                    
                    if (pattern != null)
                    {
                        capabilities.AddRange(Patterns);
                    }
                }
            }

            var confidence = _confidenceANDSeverityCalculator.CalculateOverallConfidence(DetectionSource);
            var severity = _confidenceANDSeverityCalculator.CalculateOverallSeverity(DetectionSource);
            var score = _scoringAlgorithmn.CalculateScore(confidence, severity);






            await _insert.InsertFile(Path.GetFileName(filepath), file.FileHash, userId, score, capabilities, confidence, severity);
            
            return new ScanResult
            {
                score = score,
                confidence = confidence,
                fileHash = file.FileHash,
                fileName = file.FileName,
                Sources = DetectionSource,
                severity = severity,
             
            };
            






        }
    }
}

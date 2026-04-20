namespace Toolkit_API.Domain.Entities.FileAnalysis
{
    public class FileAnalysisResult
    {
        public string FilePath { get; set; }
        public string AnalysisResult { get; set; }
        public double Score { get; set; }
        public string verdict
        {
            get
            {
                if (Score < 20.0)
                    return "Safe";
                else if (Score >= 50.0 && Score < 80.0)
                    return "Suspicious";
                else if (Score >= 80.0)
                    return "Malicious";
                else
                    return "Unknown";
            }
        }

    }
}

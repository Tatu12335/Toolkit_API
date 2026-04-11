using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Domain.Entities.FileAnalysis;

namespace Toolkit_API.Application.Application_Services.Operations
{
    public class HandleResult
    {
        
        public HandleResult() { }
        public async Task<string> HandleAsync(Response response, FileAnalysisResult analysisResult)
        {
            if (response.QueryStatus == "ok" && response.Data != null && response.Data?.Count > 0)
            {
                var malwareData = response.Data[0];
                analysisResult.AnalysisResult = "malicious";
                analysisResult.Score += 40;
                return $"The file is might be malicious. SIGNATURE : [{malwareData.Signature}]. FILE : [{malwareData.FileName}].";
            }
            else
            {
                return $" FILE : [{analysisResult.FilePath}]. SCORE : [{analysisResult.Score}].";
            }
        }
    }
}

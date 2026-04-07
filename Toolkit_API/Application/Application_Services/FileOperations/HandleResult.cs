using Toolkit_API.Domain.Entities.Files;

namespace Toolkit_API.Application.Application_Services.Operations
{
    public class HandleResult
    {
        public async Task<string> HandleAsync(Response response)
        {
            if(response.QueryStatus == "ok" && response.Data != null && response.Data?.Count > 0)
            {
                var malwareData = response.Data[0];
                return $"The file is malicious. SIGNATURE : [{malwareData.Signature}]. FILE : [{malwareData.FileName}].";
            }
            else
            {
                return "The file is clean.";
            }
        }
    }
}

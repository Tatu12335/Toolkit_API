using Toolkit_API.Domain.Entities.Files;

namespace Toolkit_API.Application.Application_Services.Operations
{
    public class HandleResult
    {

        public HandleResult() { }
        public async Task<string> HandleAsync(Response response)
        {
            if (response.QueryStatus == "ok" && response.Data != null && response.Data?.Count > 0)
            {
                var malwareData = response.Data[0];

                return $"The file is might be malicious. SIGNATURE : [{malwareData.Signature}]. FILE : [{malwareData.FileName}].";
            }
            else
            {
                return $" FILE : [placeholder]. SCORE : [placeholder].";
            }
        }
    }
}

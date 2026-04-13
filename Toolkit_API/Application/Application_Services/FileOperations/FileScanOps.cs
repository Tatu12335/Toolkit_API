using Toolkit_API.Application.Interfaces;

namespace Toolkit_API.Application.Application_Services.Operations
{
    public class FileScanOps
    {
        private readonly IFileScanRepo _repository;
        private readonly ICallExternalAPI _externalAPI;
        private readonly HandleResult _handleResult;
        private readonly StaticFileAnalysis _staticFileAnalysis;
        public FileScanOps(IFileScanRepo repository, ICallExternalAPI externalAPI, HandleResult handleResult, StaticFileAnalysis staticFileAnalysis)
        {
            _repository = repository;
            _externalAPI = externalAPI;
            _handleResult = handleResult;
            _staticFileAnalysis = staticFileAnalysis;
            
        }

        public async Task<string> ScanFile(string filePath, int userId)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if (!File.Exists(filePath))
                throw new FileNotFoundException();


            var hash = await _repository.ScanFile(filePath, userId);
            var result = await _externalAPI.CallAPI(hash, Environment.GetEnvironmentVariable("Malware_Bazaar_key"));
            var analysisResult = await _staticFileAnalysis.AnalyzeFileExtension(filePath);
            var handled = await _handleResult.HandleAsync(result,analysisResult);
            return handled;

        }


    }
}

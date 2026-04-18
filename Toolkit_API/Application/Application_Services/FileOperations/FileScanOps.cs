using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.FileAnalysis;
using Toolkit_API.Infrastructure.Services;

namespace Toolkit_API.Application.Application_Services.Operations
{
    public class FileScanOps
    {
        private readonly IFileScanRepo _repository;
        private readonly ICallExternalAPI _externalAPI;
        private readonly HandleResult _handleResult;
        private readonly StaticFileAnalysis _staticFileAnalysis;
        private readonly FileHasher _fileHasher;
        public FileScanOps(IFileScanRepo repository, ICallExternalAPI externalAPI, HandleResult handleResult, StaticFileAnalysis staticFileAnalysis, FileHasher fileHasher)
        {
            _repository = repository;
            _externalAPI = externalAPI;
            _handleResult = handleResult;
            _fileHasher = fileHasher;
            _staticFileAnalysis = staticFileAnalysis;

        }

        public async Task<string> ScanFile(string filePath, int userId)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if (!File.Exists(filePath))
                throw new FileNotFoundException();


            var hash = await _fileHasher.HashFileAsync(filePath);
            var hashExists = await _repository.DoubleHash(hash);

            if (hashExists != null)
            {
                var file = await _repository.GetFile(hash, userId);
                
                if (file != null)
                    return $"File has already been scanned. FileName: {file.FileName}, Score: {file.Score}";

            }

            var result = await _externalAPI.CallAPI(hash, Environment.GetEnvironmentVariable("Malware_Bazaar_key"));
            var handled = await _handleResult.HandleAsync(result);

            var staticAnalysisResult = await StaticScan(filePath, userId);

            await _repository.InsertAll(filePath, userId, staticAnalysisResult.Score);
            return $"Static Analysis Result: {staticAnalysisResult.AnalysisResult}, Score: {staticAnalysisResult.Score}";

        }
        public async Task<FileAnalysisResult> StaticScan(string filePath, int userId)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            var analysisResult = await _staticFileAnalysis.AnalyzeFile(filePath);
            return analysisResult;

        }
    }
}

using System.Diagnostics;
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
        private readonly IHandleDirectories _handleDirectoryResult;
        public FileScanOps(IFileScanRepo repository, 
            ICallExternalAPI externalAPI, 
            HandleResult handleResult, 
            StaticFileAnalysis staticFileAnalysis, 
            FileHasher fileHasher, 
            IHandleDirectories handleDirectoryResult)
        {
            _repository = repository;
            _externalAPI = externalAPI;
            _handleResult = handleResult;
            _fileHasher = fileHasher;
            _handleDirectoryResult = handleDirectoryResult;
            _staticFileAnalysis = staticFileAnalysis;

        }

        public async Task<string> ScanFile(string filePath, int userId)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            bool isFolder = Directory.Exists(filePath);
            
            if (isFolder)
                await HandleDirectory(filePath, userId);
            
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
        public async Task<string> HandleDirectory(string directoryPath, int userId)
        {
           var result = await _handleDirectoryResult.Handle(directoryPath);
           Debug.WriteLine($"Directory Handling Result: {result}");
           return result;
        }
    }

}

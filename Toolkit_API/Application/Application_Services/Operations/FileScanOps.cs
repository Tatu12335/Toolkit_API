using Toolkit_API.Application.Interfaces;
using Toolkit_API.Infrastructure.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Toolkit_API.Domain.Entities.Users;

namespace Toolkit_API.Application.Application_Services.Operations
{
    public class FileScanOps
    {
        private readonly IFileScanRepo _repository;
        private readonly ICallExternalAPI _externalAPI;
        private readonly HandleResult _handleResult;
        public FileScanOps(IFileScanRepo repository, ICallExternalAPI externalAPI, HandleResult handleResult)
        {
            _repository = repository;
            _externalAPI = externalAPI;
            _handleResult = handleResult;
        }

        public async Task<string> ScanFile(string filePath,int userId)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if(!File.Exists(filePath))
                throw new FileNotFoundException();
            

            var hash = await _repository.ScanFile(filePath,userId);
            var result = await _externalAPI.CallAPI(hash, Environment.GetEnvironmentVariable("Malware_Bazaar_key"));
            var handled = await _handleResult.HandleAsync(result);
            return handled;
        }


    }
}

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
        public FileScanOps(IFileScanRepo repository)
        {
            _repository = repository;
        }

        public async Task ScanFile(string filePath,int userId)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if(!File.Exists(filePath))
                throw new FileNotFoundException();
            

            await _repository.ScanFile(filePath,userId);
            

        }


    }
}

using Toolkit_API.Application.Interfaces;
using Toolkit_API.Infrastructure.Services;

namespace Toolkit_API.Application.Application_Services.Operations
{
    public class FileScanOps
    {
        private readonly IFileScanRepo _repository;
        public FileScanOps(IFileScanRepo repository)
        {
            _repository = repository;
        }

        public async Task ScanFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            if(!File.Exists(filePath))
                throw new FileNotFoundException();

            await _repository.ScanFile(filePath);


        }


    }
}

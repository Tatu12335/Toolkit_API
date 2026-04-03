namespace Toolkit_API.Application.Interfaces
{
    public interface IFileScanRepo
    {
        public Task <Toolkit_API.Domain.Entities.Files.File> ScanFile(string filePath);
    }
}

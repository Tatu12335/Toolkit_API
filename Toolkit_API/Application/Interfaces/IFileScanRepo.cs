namespace Toolkit_API.Application.Interfaces
{
    public interface IFileScanRepo
    {
        public Task  ScanFile(string filePath, int userId);
    }
}

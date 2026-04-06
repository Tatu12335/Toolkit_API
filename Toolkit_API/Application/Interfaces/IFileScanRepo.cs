namespace Toolkit_API.Application.Interfaces
{
    public interface IFileScanRepo
    {
        public Task<byte[]> ScanFile(string filePath, int userId);
    }
}

using System.IO.Compression;

namespace Toolkit_API.Application.Interfaces
{
    public interface IZipHandler
    {

        public Task<FileStream> OpenRead(string filePath);
        public Task<ZipArchive> OpenEntry(string filePath);
        public string HandleTempRoot(string filePath);
        public Task<string> ExtractFile(string DestPath, string destinationPath);
        public string CreateDestinationPath(string tempRoot, ZipArchive entry);
    }
}

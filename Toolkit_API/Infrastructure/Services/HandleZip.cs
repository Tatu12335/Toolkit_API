using System.IO.Compression;
using Toolkit_API.Application.Interfaces;

namespace Toolkit_API.Infrastructure.Services
{
    public class HandleZip : IZipHandler
    {

        public async Task<FileStream> OpenRead(string filePath)
        {
            using var fS = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return fS;
        }
        public async Task<ZipArchive> OpenEntry(string filePath)
        {
            using var zipArchive = new ZipArchive(await OpenRead(filePath), ZipArchiveMode.Read);
            return zipArchive;
        }
        public string HandleTempRoot(string filePath)
        {
            var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempRoot);
            return tempRoot;
        }
        public string CreateDestinationPath(string tempRoot, string entryFullName)
        {
            var destinationPath = Path.Combine(tempRoot, entryFullName);
            var destinationDir = Path.GetDirectoryName(destinationPath);

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            return destinationPath;
        }
        public async Task<string> ExtractFile(string path, ZipArchiveEntry entry)
        {
            using var entryStream = entry.Open();
            using var destStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            await destStream.CopyToAsync(entryStream);
            return path;
        }
    }
}

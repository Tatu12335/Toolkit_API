using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Policies;

namespace Toolkit_API.Application.Application_Services.FileOperations
{
    public class HandleZip
    {
        private readonly IZipHandler _zipHandler;
        private readonly ZipPolicies _zipPolicies;
        public HandleZip(IZipHandler zipHandler, ZipPolicies zipPolicies)
        {
            _zipHandler = zipHandler;
            _zipPolicies = zipPolicies;
        }
        public async Task<string> ProcessZip(string filepath)
        {
            if (filepath == null)
                throw new ArgumentNullException();
            if (!File.Exists(filepath))
                throw new FileNotFoundException();
            filepath = Path.GetFullPath(filepath);
            var fileInfo = new FileInfo(filepath);

            if (fileInfo.Length > _zipPolicies.MaxZipSize)
            {
                return "Zip file Exceeds the policy";
            }

            var openRead = await _zipHandler.OpenRead(filepath);
            var entries = await _zipHandler.OpenEntry(filepath);
            var tempRoot = _zipHandler.HandleTempRoot(filepath);

            if (entries.Entries.Count > _zipPolicies.MaxEntries)
            {
                return "Zip file contains too many entries.";
            }

            long curTotalUncompressedSize = 0;
            foreach (var entry in entries.Entries)
            {
                if (string.IsNullOrEmpty(entry.Name) && entry.FullName.EndsWith("/"))
                    continue;

                curTotalUncompressedSize += entry.Length;

                if (curTotalUncompressedSize > _zipPolicies.MaxZipSize)
                    return "Total uncompressed size exceeds the policy.";

                if (entry.CompressedLength > 0)
                {
                    double ratio = entry.Length / (double)entry.CompressedLength;

                    if (double.IsNaN(ratio) || double.IsInfinity(ratio) || ratio > _zipPolicies.MaxCompressionRatio)
                        return "Compression ratio exceeds the policy.";
                    
                }

                var destinationPath = Path.Combine(tempRoot, entry.FullName);

                await _zipHandler.ExtractFile(entry.ToString(), destinationPath);

                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                    return "DestinationPath exists, Deleting.";
                }

                if (Directory.Exists(tempRoot))
                {
                    Directory.Delete(tempRoot, recursive: true);
                    return "TempRoot Exists, Deleting.";
                }

                return "";

            }
            return "ZIP Handled";
        }
    }
}

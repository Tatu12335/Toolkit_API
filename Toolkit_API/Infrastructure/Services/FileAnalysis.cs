using System.Diagnostics;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.FileAnalysis;
namespace Toolkit_API.Infrastructure.Services
{
    public class FileAnalysis : IFileAnalysis
    {
        public FileAnalysis()
        {

        }
        public async Task<string> Detect(byte[] bytes) => bytes switch
        {
            [0x4D, 0x5A, ..] => "Executable (PE)",
            [0x25, 0x50, 0x44, 0x46, ..] => "PDF Document",
            [0xFF, 0xD8, 0xFF, ..] => "JPEG Image",
            [0x89, 0x50, 0x4E, 0x47, ..] => "PNG Image",
            [0x47, 0x49, 0x46, 0x38, ..] => "GIF Image",
            [0x52, 0x61, 0x72, 0x21, ..] => "RAR Archive",
            [0x50, 0x4B, 0x03, 0x04, ..] => "ZIP Archive",
            _ => "Unknown File Type"
        };

        public async Task<string> AnalyzeFile(string filePath)
        {
            var bytes = await File.ReadAllBytesAsync(filePath);
            var fileType = await Detect(bytes);

            return fileType;
        }
        public async Task<bool> ExtensionMatches(string filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException($"File not found: {filepath}");

            var extension = Path.GetExtension(filepath);
            var bytes = await File.ReadAllBytesAsync(filepath);
            var detectedType = await Detect(bytes);

            return detectedType.Contains(extension.TrimStart('.'), StringComparison.OrdinalIgnoreCase);

        }
        public async Task<bool> CheckForSuspiciousPatterns(string filePath, ExtractedStrings extractedStrings)
        {

            var bytes = await File.ReadAllBytesAsync(filePath);

            foreach (var pattern in extractedStrings.Patterns)
            {
                if (bytes.AsSpan().IndexOf(pattern) >= 0)
                {
                    Debug.WriteLine($"Suspicious pattern found in file: {filePath}");

                    return true;
                }

            }
            return false;
        }
    }
}

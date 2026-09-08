using Org.BouncyCastle.Utilities;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.FileAnalysis;
using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Domain.Policies;
namespace Toolkit_API.Infrastructure.Services
{
    public class FileAnalysis : IFileAnalysis
    {
        private readonly ICapabilityAnalyzer _analyzer;
        private readonly IImportAnalyzer _importAnalyzer;
        public FileAnalysis(ICapabilityAnalyzer analyzer, IImportAnalyzer importAnalyzer)
        {
            _analyzer = analyzer;
            _importAnalyzer = importAnalyzer;
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
        public async Task ExtensionMatches(string filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException($"File not found: {filepath}");

            var extension = Path.GetExtension(filepath);
            var bytes = await File.ReadAllBytesAsync(filepath);
            var detectedType = await Detect(bytes);

            if (!detectedType.Contains(extension.TrimStart('.'), StringComparison.OrdinalIgnoreCase))
            {

            }

        }
        // This method should only be called from the "ComboDetection" Method
        public IEnumerable<Capability> FindDetections(byte[] bytes, ExtractedStrings extractedStrings)
        {
            var byteCapabilities = new List<Capability>();
            var imports = new List<Capability>();
            foreach (var entry in extractedStrings.Patterns)
            {
                if (bytes.AsSpan().IndexOf(entry) != -1)
                {
                    var rawByteCapabilties =  _analyzer.DetectCapabilites(entry);
                    


                    if (rawByteCapabilties != null)
                        byteCapabilities.AddRange(rawByteCapabilties);
                   



                }
            }

            imports.AddRange(byteCapabilities);
            Debug.WriteLine(string.Join(", ",imports.Select(x => x.ToString())));


            return imports;
        }
        public async Task <IEnumerable<Capability>> ImportAnalysis(string filePath, ExtractedStrings extractedStrings)
        {
            var fileBytes = await File.ReadAllBytesAsync(filePath);

            var importList = new List<Capability>();
            foreach(var entry in importList)
            {
                var imports = _importAnalyzer.AnalyzeImports(fileBytes, extractedStrings);
                if(imports != null)
                    importList.AddRange(imports);

            }
            return importList;
        }
        public async Task<IEnumerable<Capability>> ComboDetection(string filePath, ExtractedStrings extractedStrings)
        {
            byte[] bytes = await File.ReadAllBytesAsync(filePath);

            IEnumerable<Capability> result = FindDetections(bytes, extractedStrings);
            
            return result;


        }

    }
}

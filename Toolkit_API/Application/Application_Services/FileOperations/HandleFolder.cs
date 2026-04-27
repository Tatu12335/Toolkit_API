using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.Domain.Entities.Files;

namespace Toolkit_API.Application.Application_Services.FileOperations
{
    public class HandleFolder
    {
        private readonly FileScanOps _ops;
        private readonly FolderInfo _files;
        public HandleFolder(FileScanOps ops,FolderInfo files)
        {
            _ops = ops;
            _files = files;
        }
        
        public async Task<FolderInfo> Handler(string path, int userId)
        {

            if (!Directory.Exists(path))
            {
                
                _files.Files.Add(path);
                await _ops.ScanFile(path, userId);
                return _files;
                
            }

            Stack<string> directories = new Stack<string>();
            directories.Push(path);
            HashSet<string> visited = new HashSet<string>();
           

            while (directories.Count > 0)
            {
                var current = directories.Pop();

                if (visited.Contains(current)) continue;
                visited.Add(current);

                if (File.Exists(current))
                {
                    var scanResult = await _ops.ScanFile(current, userId);
                    _files.Files.Add(scanResult);
                    return _files;
                }
                foreach (var file in Directory.EnumerateFiles(current, "*"))
                {

                    if (File.Exists(file))
                    {
                        var ScanResult = await _ops.ScanFile(file, userId);
                        _files.Files.Add(ScanResult);
                        
                        if(!File.Exists(file))
                            return _files;
                        
                    }
                    directories.Push(file);

                }
                var subdirectories = Directory.EnumerateDirectories(current);
                foreach (var subdirectory in subdirectories)
                {
                    directories.Push(subdirectory);
                }

            }
            
            return _files;
            

        }
    }
}

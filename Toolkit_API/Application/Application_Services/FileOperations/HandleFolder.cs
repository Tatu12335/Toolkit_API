using System.Diagnostics;
using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.Application.Interfaces;

namespace Toolkit_API.Application.Application_Services.FileOperations
{
    public class HandleFolder
    {
        private readonly FileScanOps _scanOps;
        public HandleFolder(FileScanOps scanOps) 
        { 
            _scanOps = scanOps;
        }

        public async Task<string> Handler(string path,int userId)
        {
            if (!Directory.Exists(path))
                return path;
            Debug.WriteLine("HELLO!" +  path);
            Stack<string> directories = new Stack<string>();
            directories.Push(path);

            while (directories.Count > 0)
            {
                var current = directories.Pop();

                foreach (var file in Directory.EnumerateFiles(path,"*"))
                {
                    var fileInfo = new FileInfo(file);

                    if (fileInfo.Exists)
                        path = fileInfo.FullName;

                    directories.Push(fileInfo.FullName);
                    Debug.WriteLine(fileInfo.FullName);
                    await _scanOps.ScanFile(file,userId);


                }
                var subdirectories = Directory.EnumerateDirectories(current);
                foreach (var subdirectory in subdirectories)
                {
                    directories.Push(subdirectory);
                    
                }

            }
            return path;
        }
    }
}

using Toolkit_API.Application.Interfaces;
namespace Toolkit_API.Infrastructure.Services
{
    public class HandleDirecteries : IHandleDirectories
    {
        public async Task<string> Handle(string path)
        {
            Stack<string> directories = new Stack<string>();
            directories.Push(path);

            while (directories.Count > 0)
            {
                var currentDirectory = directories.Pop();
                var files = Directory.GetFiles(currentDirectory);
                foreach (var file in files)
                {
                    return file;
                }

                var subDirectories = Directory.GetDirectories(currentDirectory);
                foreach (var subDirectory in subDirectories)
                {
                    directories.Push(subDirectory);
                    return subDirectory;
                }
                
                directories.Push(path);
            }
            return path;
        }
    }
}

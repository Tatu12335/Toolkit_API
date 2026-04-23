namespace Toolkit_API.Application.Application_Services.FileOperations
{
    public class HandleFolder
    {

        public HandleFolder() { }

        public async Task<string> Handler(string path)
        {
            if (!Directory.Exists(path))
                return path;

            Stack<string> directories = new Stack<string>();
            directories.Push(path);

            while (directories.Count > 0)
            {
                var current = directories.Pop();

                foreach (var file in Directory.EnumerateFiles(path))
                {
                    var fileInfo = new FileInfo(file);

                    if (fileInfo.Exists)
                        return fileInfo.FullName;

                    directories.Push(fileInfo.FullName);
                    return current;


                }
                var subdirectories = Directory.EnumerateDirectories(current);
                foreach (var subdirectory in subdirectories)
                {
                    directories.Push(subdirectory);
                    return subdirectory;
                }

            }
            return path;
        }
    }
}

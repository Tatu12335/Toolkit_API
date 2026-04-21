namespace Toolkit_API.Application.Interfaces
{
    public interface IHandleDirectories
    {
        public Task<string> Handle(string path);
    }
}

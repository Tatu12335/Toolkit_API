namespace Toolkit_API.Application.Interfaces
{
    public interface IAdminRepo
    {
        public Task<IEnumerable<string>> GetAllUsers();
    }
}

using Toolkit_API.Domain.Entities.Files;

namespace Toolkit_API.Application.Interfaces
{
    public interface ICallExternalAPI
    {
        public Task<Response> CallAPI(byte[] hashvalue,string apiKEY);
    }
}

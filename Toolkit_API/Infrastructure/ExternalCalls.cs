using System.Text.Json;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Files;

namespace Toolkit_API.Infrastructure
{
    public class ExternalCalls : ICallExternalAPI
    {
        private readonly HttpClient _httpClient;
        public ExternalCalls(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Response> CallAPI(byte[] hashvalue,string apiKEY)
        {
            

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Auth-Key", apiKEY);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string> ("query", "get_info"),
                new KeyValuePair<string, string> ("hash", Convert.ToHexString(hashvalue))
            });

            var response = await _httpClient.PostAsync("https://mb-api.abuse.ch/api/v1/", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result =  JsonSerializer.Deserialize<Response>(responseContent);

            return result;
        }
    }
}

using System.Text.Json.Serialization;

namespace Toolkit_API.Domain.Entities.Files
{
    public class Response
    {
        [JsonPropertyName("query-status")] public string QueryStatus { get; set; }
        [JsonPropertyName("data")] public List<MalwareData> Data { get; set; }
    }
}

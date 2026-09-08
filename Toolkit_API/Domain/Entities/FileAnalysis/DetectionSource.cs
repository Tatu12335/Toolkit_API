
using System.Text.Json.Serialization;

using Toolkit_API.Domain.Policies;

namespace Toolkit_API.Domain.Entities.FileAnalysis
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Source 
    {
        none = 0,
        Import = 1 << 0,
        String = 1 << 1,
    
    }
    public class DetectionSource
    {
        public Dictionary<Capability,Source> src { get; set; } = new Dictionary<Capability,Source>();
    }
}

using System.Text.Json.Serialization;
using Toolkit_API.Domain.Entities.FileAnalysis;
using Toolkit_API.Domain.Policies;
namespace Toolkit_API.Domain.Entities.Files
{
    public enum RiskLevel { Low, Medium, High, Critical }
    public class ScanResult
    {
        public double score { get; set; }
        public byte[] fileHash { get; set; }
        public string fileName { get; set; }
        public double confidence { get; set; } = 0.0;
        public double severity { get; set; } = 0.0;
        public RiskLevel riskLevel { get; set; } = RiskLevel.Low;
        //public IEnumerable<Capability> capabilities { get; set; } = new List<Capability>();
        public IEnumerable<DetectionSource> Sources { get; set; } 
        
    }
}

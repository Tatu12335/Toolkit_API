namespace Toolkit_API.Domain.Policies
{
    public class ZipPolicies
    {
        public long MaxZipSize { get; set; } = 100_000_000; // 100 MB in bytes
        public int MaxEntries { get; set; } = 1000; // Maximum number of entries in the zip file
        public double MaxCompressionRatio { get; set; } = 10.0; // Maximum allowed compression ratio (compressed size / original size)
    }
}

namespace Toolkit_API.Domain.Entities.Files
{
    public class FileScanLog
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public byte[] FileHash { get; set; }
        public int userId { get; set; } = 0;
        public double Score { get; set; }
    }
}

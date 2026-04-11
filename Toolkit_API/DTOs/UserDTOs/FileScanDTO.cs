using System.ComponentModel.DataAnnotations;

namespace Toolkit_API.DTOs.UserDTOs
{
    public class FileScanDTO
    {
        [Required]
        public string filePath { get; set; }
        [Required]
        public string detectionStatus { get; set; } = string.Empty;
    }
}

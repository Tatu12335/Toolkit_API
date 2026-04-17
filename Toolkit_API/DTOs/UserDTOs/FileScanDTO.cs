using System.ComponentModel.DataAnnotations;

namespace Toolkit_API.DTOs.UserDTOs
{
    public class FileScanDTO
    {
        [Required]
        public string filePath { get; set; }

    }
}

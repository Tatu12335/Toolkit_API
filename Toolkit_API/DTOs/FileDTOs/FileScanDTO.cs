using System.ComponentModel.DataAnnotations;

namespace Toolkit_API.DTOs.FIleDTOs
{
    public class FileScanDTO
    {
        [Required]
        public string filePath { get; set; }
        [Required]
        public int userId { get; set; }




    }
}

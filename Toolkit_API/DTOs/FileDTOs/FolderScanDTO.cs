using System.ComponentModel.DataAnnotations;

namespace Toolkit_API.DTOs.FileDTOs
{
    public class FolderScanDTO
    {
        [Required]
        public string filepath { get; set; }
    }
}

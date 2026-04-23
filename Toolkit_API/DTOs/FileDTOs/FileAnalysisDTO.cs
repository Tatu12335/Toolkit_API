using System.ComponentModel.DataAnnotations;

namespace Toolkit_API.DTOs.FIleDTOs
{
    public class FileAnalysisDTO
    {
        [Required]
        public string FilePath { get; set; }
    }
}

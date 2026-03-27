using System.ComponentModel.DataAnnotations;

namespace Toolkit_API.DTOs.UserDTOs
{
    public class GetUserDTO
    {
        [Required]
        public string username { get; set; } = string.Empty;
    }
}

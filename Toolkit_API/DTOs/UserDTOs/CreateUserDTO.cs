using System.ComponentModel.DataAnnotations;

namespace Toolkit_API.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}

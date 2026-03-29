using System.ComponentModel.DataAnnotations;

namespace Toolkit_API.DTOs.UserDTOs
{
    public class LoginDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        //public string email { get; set; }

    }
}

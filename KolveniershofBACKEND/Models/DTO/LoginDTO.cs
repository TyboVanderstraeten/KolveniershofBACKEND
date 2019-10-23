using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class LoginDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}

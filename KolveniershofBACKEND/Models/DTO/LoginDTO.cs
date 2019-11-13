using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class LoginDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail is required")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}

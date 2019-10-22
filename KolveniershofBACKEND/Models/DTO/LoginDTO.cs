using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

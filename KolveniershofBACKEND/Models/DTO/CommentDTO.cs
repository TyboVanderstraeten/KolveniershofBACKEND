using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.DTO
{
    public class CommentDTO
    {
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
    }
}

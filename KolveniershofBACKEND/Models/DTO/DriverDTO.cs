using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class DriverDTO
    {
        [Required]
        public string Name { get; set; }
    }
}

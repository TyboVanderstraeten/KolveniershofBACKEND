using KolveniershofBACKEND.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class ActivityDTO
    {
        public int ActivityId { get; set; }
        [Required(ErrorMessage = "ActivityType is required")]
        public ActivityType ActivityType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pictogram is required")]
        public string Pictogram { get; set; }
    }
}

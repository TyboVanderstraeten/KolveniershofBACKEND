using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class DayDTO
    {
        public int DayId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Template name is required")]
        public string TemplateName { get; set; }
        [Required(ErrorMessage = "Weeknr is required")]
        public int WeekNr { get; set; }
        [Required(ErrorMessage = "Daynr is required")]
        public int DayNr { get; set; }
        public ICollection<DayActivityDTO> DayActivities { get; set; }
        public ICollection<HelperDTO> Helpers { get; set; }
    }
}

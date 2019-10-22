using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.DTO
{
    public class ActivityDTO
    {
        public int ActivityId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Pictogram { get; set; }
    }
}

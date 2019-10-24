﻿using KolveniershofBACKEND.Models.Domain;

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
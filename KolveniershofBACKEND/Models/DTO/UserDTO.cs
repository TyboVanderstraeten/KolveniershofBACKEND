using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "UserType is required")]
        public UserType UserType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int? Group { get; set; }
        public int? DegreeOfLimitation { get; set; }
        public ICollection<WeekendDayDTO> CustomWeekendDays { get; set; }
    }
}

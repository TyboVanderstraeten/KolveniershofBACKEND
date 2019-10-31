using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int? Group { get; set; }
        public ICollection<CustomWeekendDayDTO> CustomWeekendDays { get; set; }
    }
}

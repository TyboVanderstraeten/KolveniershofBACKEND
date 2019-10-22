using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public int? Group { get; set; }
        public ICollection<Attendance> Attendances { get; set; }

        public UserDTO(User user)
        {
            UserId = user.UserId;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfilePicture = user.ProfilePicture;
            Group = user.Group;
            Attendances = user.Attendances;
        }

    }
}

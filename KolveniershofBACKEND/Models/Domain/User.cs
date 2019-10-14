using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public int? Group { get; set; }

        protected User()
        {

        }

        public User(UserType userType, string firstName, string lastName, string profilePicture, int? group)
        {
            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
            ProfilePicture = profilePicture;
            Group = group;
        }

    }
}

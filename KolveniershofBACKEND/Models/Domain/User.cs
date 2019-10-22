using System.Collections.Generic;

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
        public ICollection<Attendance> Attendances { get; set; }

        public string Username { get => $"{FirstName.Replace(' ', '_')}.{LastName.Replace(' ', '_')}"; }

        protected User()
        {
            Attendances = new List<Attendance>();
        }

        public User(UserType userType, string firstName, string lastName, string profilePicture, int? group)
        {
            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
            ProfilePicture = profilePicture;
            Group = group;
            Attendances = new List<Attendance>();
        }
    }
}

using System;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int? Group { get; set; }
        public ICollection<Attendance> Attendances { get; set; }

        protected User()
        {
            Attendances = new List<Attendance>();
        }

        public User(UserType userType, string firstName, string lastName, string email, string profilePicture, int? group)
        {
            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ProfilePicture = profilePicture;
            Group = group;
            Attendances = new List<Attendance>();
        }

        public void AddAttendance(Attendance attendance)
        {
            Attendances.Add(attendance);
        }

        public void RemoveAttendance(Attendance attendance)
        {
            Attendances.Remove(attendance);
        }
    }
}

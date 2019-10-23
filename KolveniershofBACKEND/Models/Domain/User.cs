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
        public DateTime Birthdate { get; set; }
        public string ProfilePicture { get; set; }
        public int? Group { get; set; }
        public ICollection<Attendance> Attendances { get; set; }

        public string Username { get => $"{FirstName.Replace(" ", string.Empty)}{LastName.Replace(" ", string.Empty)}{Birthdate.Year}"; }

        protected User()
        {
            Attendances = new List<Attendance>();
        }

        public User(UserType userType, string firstName, string lastName, DateTime birthdate, string profilePicture, int? group)
        {
            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
            Birthdate = birthdate;
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

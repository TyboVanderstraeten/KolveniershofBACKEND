namespace KolveniershofBACKEND.Models.Domain
{
    public class Helper
    {
        public int DayId { get; set; }
        public int UserId { get; set; }
        public Day Day { get; set; }
        public User User { get; set; }

        protected Helper()
        {

        }

        public Helper(Day day, User user)
        {
            DayId = day.DayId;
            UserId = user.UserId; 
            Day = day;
            User = user;
        }
    }
}

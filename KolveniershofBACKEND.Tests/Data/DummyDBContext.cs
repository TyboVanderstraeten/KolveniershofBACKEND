using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;


namespace KolveniershofBACKEND.Tests.Data
{


    public class DummyDBContext
    {

        #region Users
        public User U1 { get; }
        public User U2 { get; }
        public User U3 { get; }
        public User[] Users { get; }
        #endregion

        #region Activities
        public Activity Activity1 { get; }
        public Activity Activity2 { get; }
        public Activity Activity3 { get; }
        public Activity Activity4 { get; }
        public Activity Activity5 { get; }
        public Activity Activity6 { get; }
        public Activity Activity7 { get; }
        public Activity[] Activities { get; }
        #endregion

        #region TemplateDay
        private Day Day1 { get; }
        private Day Day2 { get; }
        private Day Day3 { get; }
        private Day[] Days { get; }

        private DayActivity DayActivity1 { get; }
        private DayActivity DayActivity2 { get; }
        private DayActivity DayActivity3 { get; }
        private DayActivity DayActivity4 { get; }

        private DayActivity DayActivity5 { get; }
        private DayActivity DayActivity6 { get; }
        private DayActivity DayActivity7 { get; }
        private DayActivity DayActivity8 { get; }

        private DayActivity DayActivity9 { get; }
        private DayActivity DayActivity10 { get; }
        private DayActivity DayActivity11 { get; }
        private DayActivity DayActivity12 { get; }

        private DayActivity[] DayActivities1 { get; }
        private DayActivity[] DayActivities2 { get; }
        private DayActivity[] DayActivities3 { get; }

        private Helper Helper1 { get; }
        private Helper Helper2 { get; }
        private Helper[] Helpers1 { get; }

        private Helper Helper3 { get; }
        private Helper Helper4 { get; }
        private Helper[] Helpers2 { get; }

        private Helper Helper5 { get; }
        private Helper Helper6 { get; }
        private Helper[] Helpers3 { get; }  


        #endregion


        public DummyDBContext()
        {
            #region init Users
            int userId = 1;
            U1 = new User(UserType.BEGELEIDER, "Tybo", "Vanderstraeten", "tybo@hotmail.com", "string.jpeg", null) { UserId = userId++ };
            U2 = new User(UserType.CLIENT, "Rob", "De Putter", "rob@hotmail.com", "string.jpeg", 2) { UserId = userId++ };
            U3 = new User(UserType.STAGIAIR, "Tim", "Geldof", "tim@hotmail.com", "string.jpeg", null) { UserId = userId++ };

            Users = new[] { U1, U2, U3 };
            #endregion

            #region init Activities
            int activityId = 1;

            Activity1 = new Activity(ActivityType.ATELIER, "Testatelier", "Dit is een testatelier", "test.picto") { ActivityId = activityId++ };
            Activity2 = new Activity(ActivityType.ATELIER, "Koken", "We gaan koken", "koken.picto") { ActivityId = activityId++ };
            Activity3 = new Activity(ActivityType.ATELIER, "Zwemmen", "Baantjes zwemmen", "zwemmen.picto") { ActivityId = activityId++ };
            Activity4 = new Activity(ActivityType.AFWEZIG, "Afwezig", "Afwezig", "afwezig.picto") { ActivityId = activityId++ };
            Activity5 = new Activity(ActivityType.ZIEK, "Ziek", "Ziek", "ziek.picto") { ActivityId = activityId++ };
            Activity6 = new Activity(ActivityType.VERVOER, "Bus", "Bus", "bus.picto") { ActivityId = activityId++ };
            Activity7 = new Activity(ActivityType.KOFFIE, "Koffie", "Koffie", "Koffie.picto") { ActivityId = activityId++ };

            Activities = new[] { Activity1, Activity2, Activity3, Activity4, Activity5, Activity6, Activity7 };
            #endregion

            #region init Days
            int dayId = 1;

            Day1 = new Day(1, 1) { DayId = dayId++ };
            Day2 = new Day(1, 2) { DayId = dayId++ };
            Day3 = new Day(1, 3) { DayId = dayId++ };

            DayActivity1 = new DayActivity(Day1, Activity1, TimeOfDay.VOORMIDDAG);
            DayActivity2 = new DayActivity(Day1, Activity2, TimeOfDay.NAMIDDAG);
            DayActivity3 = new DayActivity(Day1, Activity3, TimeOfDay.OCHTEND);
            DayActivity4 = new DayActivity(Day1, Activity4, TimeOfDay.AVOND);
            DayActivities1 = new[] { DayActivity1, DayActivity2, DayActivity3, DayActivity4 };
            Day1.DayActivities = DayActivities1;

            Helper1 = new Helper(Day1, U1);
            Helper2 = new Helper(Day1, U3);
            Helpers1 = new[] { Helper1, Helper2 };
            Day1.Helpers = Helpers1;

            DayActivity5 = new DayActivity(Day2, Activity5, TimeOfDay.VOORMIDDAG);
            DayActivity6 = new DayActivity(Day2, Activity7, TimeOfDay.MIDDAG);
            DayActivity7 = new DayActivity(Day2, Activity1, TimeOfDay.OCHTEND);
            DayActivity8 = new DayActivity(Day2, Activity2, TimeOfDay.AVOND);
            DayActivities2 = new[] { DayActivity5, DayActivity6, DayActivity7, DayActivity8 };
            Day2.DayActivities = DayActivities2;

            Helper3 = new Helper(Day2, U1);
            Helper4 = new Helper(Day2, U3);
            Helpers2 = new[] { Helper3, Helper4 };
            Day2.Helpers = Helpers2;

            DayActivity9 = new DayActivity(Day3, Activity3, TimeOfDay.VOORMIDDAG);
            DayActivity10 = new DayActivity(Day3, Activity4, TimeOfDay.NAMIDDAG);
            DayActivity11 = new DayActivity(Day3, Activity5, TimeOfDay.OCHTEND);
            DayActivity12 = new DayActivity(Day3, Activity6, TimeOfDay.AVOND);
            DayActivities3 = new[] { DayActivity9, DayActivity10, DayActivity11, DayActivity12 };
            Day3.DayActivities = DayActivities3;

            Helper5 = new Helper(Day3, U1);
            Helper6 = new Helper(Day3, U3);
            Helpers3 = new[] { Helper5, Helper6 };
            Day3.Helpers = Helpers3; 
            #endregion

        }


    }
}

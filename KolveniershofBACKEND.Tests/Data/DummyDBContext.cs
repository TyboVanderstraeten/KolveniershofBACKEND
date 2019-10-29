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




        }


    }
}

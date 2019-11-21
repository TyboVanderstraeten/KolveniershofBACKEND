using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.Domain.Enums;
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
        public Day Day1 { get; }
        public Day Day2 { get; }
        public Day Day3 { get; }
        public Day[] Days { get; }

        public DayActivity DayActivity1 { get; }
        public DayActivity DayActivity2 { get; }
        public DayActivity DayActivity3 { get; }
        public DayActivity DayActivity4 { get; }

        public DayActivity DayActivity5 { get; }
        public DayActivity DayActivity6 { get; }
        public DayActivity DayActivity7 { get; }
        public DayActivity DayActivity8 { get; }

        public DayActivity DayActivity9 { get; }
        public DayActivity DayActivity10 { get; }
        public DayActivity DayActivity11 { get; }
        public DayActivity DayActivity12 { get; }

        public IList<DayActivity> DayActivities1 { get; }
        public IList<DayActivity> DayActivities2 { get; }
        public IList<DayActivity> DayActivities3 { get; }

        public Helper Helper1 { get; }
        public Helper Helper2 { get; }
        public IList<Helper> Helpers1 { get; }

        public Helper Helper3 { get; }
        public Helper Helper4 { get; }
        public Helper[] Helpers2 { get; }

        public Helper Helper5 { get; }
        public Helper Helper6 { get; }
        public Helper[] Helpers3 { get; }


        #endregion

        #region CustomDay
        public CustomDay CustomDay1 { get; }
        public CustomDay CustomDay2 { get; }
        public CustomDay CustomDay3 { get; }
        public CustomDay[] CustomDays { get; }

        public Attendance Attendance1 { get; }
        public Attendance Attendance2 { get; }
        public Attendance Attendance3 { get; }
        public Attendance Attendance4 { get; }
        public Attendance Attendance5 { get; }
        public Attendance Attendance6 { get; }


        public IList<Attendance> Attendances1 { get; }
        public IList<Attendance> Attendances2 { get; }
        public IList<Attendance> Attendances3 { get; }
        public IList<Attendance> Attendances4 { get; }



        public Note Note1 { get; }
        public Note Note2 { get; }
        public IList<Note> Notes { get; }
        #endregion

        #region Driver
        public Driver Driver1 { get; }
        public Driver Driver2 { get; }
        public Driver Driver3 { get; }
        #endregion

        #region BusDriver
        public BusDriver BusDriver1 { get; }
        public BusDriver BusDriver2 { get; }
        public BusDriver BusDriver4 { get; }
        public BusDriver BusDriver3 { get; }

        public List<BusDriver> BusDriversForWeek1 = new List<BusDriver>();
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

            Day1 = new Day("eerste_week_eerste_dag",1, 1) { DayId = dayId++ };
            Day2 = new Day("eerste_week_tweede_dag",1, 2) { DayId = dayId++ };
            Day3 = new Day("eerste_week_derde_dag",1, 3) { DayId = dayId++ };
          

            Attendances1 = new List<Attendance>();
            Attendances2 = new List<Attendance>();
            Attendances3 = new List<Attendance>();
            Attendances4 = new List<Attendance>();

            
            DayActivity1 = new DayActivity(Day1, Activity5, TimeOfDay.VOLLEDIG);

            Attendance1 = new Attendance(DayActivity1, U1);
            Attendance5 = new Attendance(DayActivity1, U2);
            Attendance6 = new Attendance(DayActivity1, U2);
            Attendances1.Add(Attendance1);
            Attendances1.Add(Attendance5);
            Attendances1.Add(Attendance6);
            DayActivity1.Attendances = Attendances1;

            DayActivity2 = new DayActivity(Day1, Activity2, TimeOfDay.NAMIDDAG);

            Attendance2 = new Attendance(DayActivity2, U1);
            Attendances2.Add(Attendance2);
            DayActivity2.Attendances = Attendances2;

            DayActivity3 = new DayActivity(Day1, Activity3, TimeOfDay.OCHTEND);

            Attendance3 = new Attendance(DayActivity3, U1);
            Attendances3.Add(Attendance3);
            DayActivity3.Attendances = Attendances3;

            DayActivity4 = new DayActivity(Day1, Activity4, TimeOfDay.AVOND);

            Attendance4 = new Attendance(DayActivity4, U1);
            Attendances4.Add(Attendance4);
            DayActivity4.Attendances = Attendances4;

            DayActivities1 = new List<DayActivity>();
            DayActivities1.Add(DayActivity1);
            DayActivities1.Add(DayActivity2);
            DayActivities1.Add(DayActivity3);
            DayActivities1.Add(DayActivity4);
            Day1.DayActivities = DayActivities1;

            Helper1 = new Helper(Day1, U1);
            Helper2 = new Helper(Day1, U3);
            Helpers1 = new List<Helper>();
            Helpers1.Add(Helper1);
            Helpers1.Add(Helper2);

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



            Days = new[] { Day1, Day2, Day3 };
            #endregion

            #region init CustomDays
            Note1 = new Note(NoteType.VERVOER, "Florian neemt de bus niet vandaag");
            Note2 = new Note(NoteType.VARIA, "Vandaag zullen er geen bekertjes aanwezig zijn");
            Notes = new List<Note>();
            Notes.Add(Note1);
            Notes.Add(Note2);

            CustomDay1 = new CustomDay("eerste_week_eerste_dag",1, 1, DateTime.Today, "Wortelsoep", "Kip zoetzuur", "chocomousse");
            CustomDay1.DayActivities = DayActivities1;
            CustomDay1.Helpers = Helpers1;
            CustomDay1.Notes = Notes;
            

            CustomDay2 = new CustomDay("eerste_week_tweede_dag", 1, 2, DateTime.Today.AddDays(1), "Wortelsoep", "Kip zoetzuur", "chocomousse");
            CustomDay2.DayActivities = DayActivities2;
            CustomDay2.Helpers = Helpers2;
            CustomDay2.Notes = Notes;

            CustomDay3 = new CustomDay("eerste_week_derde_dag",1, 3, DateTime.Today.AddDays(2), "Wortelsoep", "Kip zoetzuur", "chocomousse");
            CustomDay3.DayActivities = DayActivities3;
            CustomDay3.Helpers = Helpers3;
            CustomDay3.Notes = Notes;

            CustomDays = new[] { CustomDay1, CustomDay2, CustomDay3 };
            #endregion

            #region init Drivers
            int driverId = 1;
            Driver1 = new Driver("Jan") { DriverId = driverId++ };
            Driver2 = new Driver("Karel") { DriverId = driverId++ }; ;
            Driver3 = new Driver("Makker") { DriverId = driverId++ }; ;
            #endregion

            #region init BusDriver
            BusDriver1 = new BusDriver(Day1, Driver1, TimeOfDay.OCHTEND, BusColor.BEIGE);
            BusDriver2 = new BusDriver(Day2, Driver2, TimeOfDay.AVOND, BusColor.GEEL);
            BusDriver3 = new BusDriver(Day3, Driver2, TimeOfDay.AVOND, BusColor.GEEL);
            BusDriver4 = new BusDriver(Day1, Driver2, TimeOfDay.AVOND, BusColor.GEEL);

            BusDriversForWeek1.Add(BusDriver1);
            BusDriversForWeek1.Add(BusDriver2);
            BusDriversForWeek1.Add(BusDriver3);
            BusDriversForWeek1.Add(BusDriver4);
            #endregion
        }


    }
}

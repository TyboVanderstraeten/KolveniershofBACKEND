using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;


namespace KolveniershofBACKEND.Tests.Data
{


    public class DummyDBContext
    {
        public User U1 { get; }
        public User U2{ get; }
        public User U3 { get; }
        public User[] Users { get; } 

        public DummyDBContext()
        {
            int userId = 1;
            U1 = new User(UserType.BEGELEIDER, "Tybo", "Vanderstraeten", "tybo@hotmail.com", "string.jpeg", null) { UserId = userId++};
            U2 = new User(UserType.CLIENT, "Rob", "De Putter", "rob@hotmail.com", "string.jpeg", 2) { UserId = userId++};
            U3 = new User(UserType.STAGIAIR, "Tim", "Geldof", "tim@hotmail.com", "string.jpeg", null) { UserId = userId++};

            Users = new[] { U1, U2, U3 };



        }


    }
}

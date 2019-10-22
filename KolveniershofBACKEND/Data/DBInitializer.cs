using KolveniershofBACKEND.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace KolveniershofBACKEND.Data
{
    public class DBInitializer
    {
        private DBContext _dbContext;
        private UserManager<IdentityUser> _userManager;

        public DBInitializer(DBContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public void seedDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                #region Activities
                Activity a1 = new Activity(ActivityType.ATELIER, "Testatelier", "Dit is een testatelier", "test.picto");
                _dbContext.Activities.Add(a1);
                #endregion

                _dbContext.SaveChanges();
            }
        }
    }
}

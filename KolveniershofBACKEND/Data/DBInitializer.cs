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

            }
        }
    }
}

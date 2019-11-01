using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class UsersControllerTest
    {

        private Mock<IUserRepository> _userRepository;
        private UsersController _controller;
        private DummyDBContext _dummyDBContext;
        private Mock<UserManager<IdentityUser>> _userManager;
        private Mock<SignInManager<IdentityUser>> _signInManager;
        private Mock<IConfiguration> _configuration;

        private ClaimsPrincipal user;
        private IdentityUser identityUser;
        private IdentityUser wrongIdentityUser;
        private Mock<ControllerContext> _controllerContext;

        public UsersControllerTest()
        {
            _dummyDBContext = new DummyDBContext();
            _userRepository = new Mock<IUserRepository>();
            _userManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<IdentityUser>>().Object,
              new IUserValidator<IdentityUser>[0],
              new IPasswordValidator<IdentityUser>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<IdentityUser>>>().Object);

            _controllerContext = new Mock<ControllerContext>();

            _signInManager = new Mock<SignInManager<IdentityUser>>(_userManager.Object,
                     new Mock<IHttpContextAccessor>().Object,
                     new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                     new Mock<IAuthenticationSchemeProvider>().Object);

            _configuration = new Mock<IConfiguration>();

            user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, _dummyDBContext.U2.Email),
            }));

            identityUser = new IdentityUser() { UserName = _dummyDBContext.U2.Email, Email = _dummyDBContext.U2.Email };
            wrongIdentityUser = new IdentityUser() { UserName = _dummyDBContext.U1.Email , Email = _dummyDBContext.U1.Email};


            _controller = new UsersController(_signInManager.Object, _userManager.Object, _configuration.Object, _userRepository.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
        }

        #region GetLoggedInUsers
        [Fact]
        public void GetLoggedInUser_Succeeds()
        {
            User Tybo = _dummyDBContext.U2;

            _userRepository.Setup(u => u.GetByEmail(identityUser.UserName)).Returns(_dummyDBContext.U2);

            ActionResult<User> actionResult = _controller.GetLoggedInUser();
            User user2 = actionResult.Value;
            Assert.Equal(user2.Email, Tybo.Email);
        }

        [Fact]
        public void GetLoggedInUser_Fails_UserNotKnown()
        {
            _userRepository.Setup(u => u.GetByEmail(wrongIdentityUser.UserName)).Returns((User)null);

            ActionResult<User> actionResult = _controller.GetLoggedInUser();
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }
        #endregion

        #region GetAll
        [Fact]
        public void GetAllUsers_Succeeds()
        {
            _userRepository.Setup(u => u.GetAll()).Returns(_dummyDBContext.Users);
            ActionResult<IEnumerable<User>> actionResult = _controller.GetAll();
            IList<User> users = actionResult.Value as IList<User>;

            Assert.Equal(3, users.Count);
        }
        #endregion

        #region GetAllFromGroup
        [Fact]
        public void GetAllFromGroup_Succeeds()
        {
            int groupNr = 2;
            IEnumerable<User> group2 = _dummyDBContext.Users.ToList().Where(u => u.Group == groupNr);
            _userRepository.Setup(a => a.GetAllFromGroup(groupNr)).Returns(group2);
            ActionResult<IEnumerable<User>> actionResult = _controller.GetAllFromGroup(groupNr);
            IList<User> users = actionResult.Value as IList<User>;
            Assert.Equal("Rob", users.ToList().First().FirstName); //there is only 1 user in this list!
        }
        #endregion

        #region GetAllWithType
        [Fact]
        public void GetAllWithType_Succeeds()
        {
            UserType userType = UserType.CLIENT;
            IEnumerable<User> clientUsers = _dummyDBContext.Users.ToList().Where(u => u.UserType == userType);
            _userRepository.Setup(u => u.GetAllWithType(userType)).Returns(clientUsers);

            ActionResult<IEnumerable<User>> actionResult = _controller.GetAllWithType("client");
            IList<User> users = actionResult.Value as IList<User>;

            Assert.Equal("Rob", users.ToList().First().FirstName); // there's only 1 user in this list
        }

        [Fact]
        public void GetAllWithType_Fails_TypeDoesNotExist()
        {
            ActionResult<IEnumerable<User>> actionResult = _controller.GetAllWithType("wrongType");
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetUserById_Succeeds()
        {
            int userId = 1;
            _userRepository.Setup(u => u.GetById(userId)).Returns(_dummyDBContext.U1);
            ActionResult<User> actionResult = _controller.GetById(userId);
            User user = actionResult.Value;

            Assert.Equal("Tybo", user.FirstName);
        }

        [Fact]
        public void GetUserById_ReturnsNull_UserDoesNotExist()
        {
            int userId = 100;
            _userRepository.Setup(u => u.GetById(userId)).Returns((User)null);
            ActionResult<User> actionResult = _controller.GetById(userId);
            User user = actionResult.Value;

            Assert.Null(user);
        } 
        #endregion
    }
}

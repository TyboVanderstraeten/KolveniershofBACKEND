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
                new Claim(ClaimTypes.NameIdentifier, _dummyDBContext.U2.Email),
            }));

            identityUser = new IdentityUser() { UserName = _dummyDBContext.U2.Email, Email = _dummyDBContext.U2.Email };
            wrongIdentityUser = new IdentityUser() { UserName = _dummyDBContext.U2.Email , Email = _dummyDBContext.U1.Email};


            _controller = new UsersController(_signInManager.Object, _userManager.Object, _configuration.Object, _userRepository.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
        }

        [Fact]
        public void GetLoggedInUser_Succeeds()
        {
            User Tybo = _dummyDBContext.U1;

            _userRepository.Setup(u => u.GetByEmail(identityUser.Email)).Returns(_dummyDBContext.U2);

            ActionResult<User> actionResult =  _controller.GetLoggedInUser();
            User user2 = actionResult.Value;
            Assert.Equal(user2.Email, Tybo.Email);
        }





    }
}

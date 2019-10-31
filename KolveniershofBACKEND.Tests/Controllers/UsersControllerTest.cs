using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Tests.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class UsersControllerTest
    {

        //public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
        //   IConfiguration configuration, IUserRepository userRepository)

        private Mock<IUserRepository> _userRepository;
        private UsersController _controller;
        private DummyDBContext _dummyDBContext;
        private Mock<UserManager<IdentityUser>> _userManager;
        private Mock<SignInManager<IdentityUser>> _signInManager;
        private Mock<IConfiguration> _configuration;

        private ClaimsPrincipal user;
        private IdentityUser identityUser;
        private IdentityUser wrongIdentityUser;


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

            _signInManager = new Mock<SignInManager<IdentityUser>>(_userManager.Object,
                     new Mock<IHttpContextAccessor>().Object,
                     new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                     new Mock<IAuthenticationSchemeProvider>().Object);

            _configuration = new Mock<IConfiguration>();

            user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _dummyDBContext.U1.Email),
            }));

            identityUser = new IdentityUser() { Email = _dummyDBContext.U1.Email };
            wrongIdentityUser = new IdentityUser() { Email = _dummyDBContext.U2.Email };


            _controller = new UsersController(_signInManager.Object, _userManager.Object,_configuration.Object, _userRepository.Object);
        }


    }
}

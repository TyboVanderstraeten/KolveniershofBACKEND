using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Controllers
{
    [ApiController]
    [Route("KolveniershofAPI/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IConfiguration configuration, IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("Current")]
        public ActionResult<User> GetLoggedInUser()
        {
            string username = User.Identity.Name;
            User user = _userRepository.GetByUsername(username);

            if (user == null)
            {
                return NotFound("We couldn't find the user you're looking for");
            }
            return user;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return _userRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("Group/{Id}")]
        public ActionResult<IEnumerable<User>> GetAllFromGroup(int id)
        {
            return _userRepository.GetAllFromGroup(id).ToList();
        }

        [HttpGet]
        [Route("{Id}/Attendances")]
        public ActionResult<IEnumerable<Attendance>> GetAttendancesFromUser(int id)
        {
            return _userRepository.GetAttendancesFromUser(id).ToList();
        }

        [HttpGet]
        [Route("{Id}")]
        public ActionResult<User> GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        [HttpGet]
        [Route("{Username}")]
        public ActionResult<User> GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login(LoginDTO model)
        {
            IdentityUser user = await GetUser(model.Username);
            if (user != null)
            {
                if (await CheckPassword(user, model.Password))
                {
                    string token = await GetToken(user);
                    return Created("", token);
                }
            }
            return BadRequest("Username or password is incorrect");
        }

        private async Task<string> GetToken(IdentityUser user)
        {
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };
            claims.AddRange(await _userManager.GetClaimsAsync(user));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                null, null, claims.ToArray(),
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<IdentityUser> GetUser(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        private async Task<bool> CheckPassword(IdentityUser identityUser, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false);
            return result.Succeeded;
        }
    }
}
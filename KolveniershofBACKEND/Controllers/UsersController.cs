using KolveniershofBACKEND.Data;
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
        [Route("current")]
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
        [Route("all")]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return _userRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("group/{id}")]
        public ActionResult<IEnumerable<User>> GetAllFromGroup(int id)
        {
            return _userRepository.GetAllFromGroup(id).ToList();
        }

        [HttpGet]
        [Route("type/{type}")]
        public ActionResult<IEnumerable<User>> GetAllWithType(string type)
        {
            if (Enum.IsDefined(typeof(UserType), type.ToUpper()))
            {
                return _userRepository.GetAllWithType((UserType)Enum.Parse(typeof(UserType), type.ToUpper())).ToList();
            }
            else
            {
                return BadRequest("Type is incorrect");
            }
        }

        [HttpGet]
        [Route("{id}/attendances")]
        public ActionResult<IEnumerable<Attendance>> GetAttendancesFromUser(int id)
        {
            return _userRepository.GetAttendancesFromUser(id).ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<User> GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        [HttpGet]
        [Route("username/{username}")]
        public ActionResult<User> GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        [HttpPost]
        [Route("login")]
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

        [HttpPost]
        [Route("new")]
        public async Task<ActionResult<User>> Add(UserDTO model)
        {
            try
            {
                User userToCreate = new User(
                    model.UserType,
                    model.FirstName,
                    model.LastName,
                    model.Birthdate,
                    model.ProfilePicture,
                    model.Group);

                //Temp
                if (_userRepository.GetByUsername(userToCreate.Username) != null)
                {
                    return BadRequest("User already exists");
                }

                IdentityUser identityUserToCreate = new IdentityUser()
                {
                    UserName = userToCreate.Username,
                    LockoutEnabled = true
                };

                await _userManager.CreateAsync(identityUserToCreate, "P@ssword1");
                _userRepository.Add(userToCreate);
                _userRepository.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = userToCreate.UserId }, userToCreate);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Works 90% off the time, sometimes produces an error: using result of async call to edit somewhere else, doesn't always happen fast enough
        // NEED TO FIX
        [HttpPut]
        [Route("edit")]
        public ActionResult<User> Edit(UserDTO model)
        {
            User userToEdit = _userRepository.GetById(model.UserId);
            IdentityUser identityUserToEdit = GetUser(userToEdit.Username).GetAwaiter().GetResult();
            userToEdit.UserType = model.UserType;
            userToEdit.FirstName = model.FirstName;
            userToEdit.LastName = model.LastName;
            userToEdit.Birthdate = model.Birthdate;
            userToEdit.ProfilePicture = model.ProfilePicture;
            userToEdit.Group = model.Group;

            identityUserToEdit.UserName = userToEdit.Username;
            _userRepository.SaveChanges();
            return Ok(userToEdit);
        }

        // Works 90% off the time, sometimes produces an error: using result of async call to remove user, doesn't always happen fast enough
        // NEED TO FIX
        [HttpDelete]
        [Route("remove/{id}")]
        public async Task<ActionResult<User>> Remove(int id)
        {
            User userToDelete = _userRepository.GetById(id);

            if (userToDelete == null)
            {
                return NoContent();
            }
            else
            {
                IdentityUser identityUserToDelete = GetUser(userToDelete.Username).GetAwaiter().GetResult();
                await _userManager.DeleteAsync(identityUserToDelete);
                _userRepository.Remove(userToDelete);
                _userRepository.SaveChanges();
                return Ok(userToDelete);
            }
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
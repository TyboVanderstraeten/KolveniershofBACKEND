using KolveniershofBACKEND.Data;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ICustomDayRepository _customDayRepository;

        public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IConfiguration configuration, IUserRepository userRepository, ICustomDayRepository customDayRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _userRepository = userRepository;
            _customDayRepository = customDayRepository;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>All users</returns>
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return _userRepository.GetAll().ToList();
        }

        /// <summary>
        /// Get all users from a group
        /// </summary>
        /// <param name="groupId">The id of the group</param>
        /// <returns>The users of the group</returns>
        [HttpGet]
        [Route("group/{id}")]
        public ActionResult<IEnumerable<User>> GetAll(int groupId)
        {
            return _userRepository.GetAllFromGroup(groupId).ToList();
        }

        /// <summary>
        /// Get all users with a specific type
        /// </summary>
        /// <param name="type">The type of the user</param>
        /// <returns>The users with the type</returns>
        [HttpGet]
        [Route("{type}")]
        public ActionResult<IEnumerable<User>> GetAll(string type)
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

        /// <summary>
        /// Get a specific user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The user</returns>
        [HttpGet]
        [Route("{userId}")]
        public ActionResult<User> GetById(int userId)
        {
            return _userRepository.GetById(userId);
        }

        /// <summary>
        /// Get a specific user
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>The user</returns>
        [HttpGet]
        [Route("{email}")]
        public ActionResult<User> GetByEmail(string email)
        {
            // Only if user is client or begeleider
            return _userRepository.GetByEmail(email);
        }

        /// <summary>
        /// Check if an email is available
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>True: available | False: unavailable</returns>
        [HttpGet]
        [Route("availability/{email}")]
        public async Task<ActionResult<bool>> CheckAvailibilityEmail(string email)
        {
            IdentityUser identityUser = await _userManager.FindByEmailAsync(email);
            return identityUser == null;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model">The user's credentials</param>
        /// <returns>A token</returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(LoginDTO model)
        {
            IdentityUser user = await GetUser(model.Email);
            if (user != null)
            {
                if (await CheckPassword(user, model.Password))
                {
                    string token = await GetToken(user);
                    return Created("", token);
                }
            }
            return BadRequest("E-mail or password is incorrect");
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model">The user</param>
        /// <returns>The user</returns>
        [HttpPost]
        public async Task<ActionResult<User>> Add(UserDTO model)
        {
            try
            {
                User userToCreate = new User(
                    model.UserType,
                    model.FirstName,
                    model.LastName,
                    model.Email,
                    model.ProfilePicture,
                    model.Group,
                    model.DegreeOfLimitation);

                if (_userRepository.GetByEmail(userToCreate.Email) != null)
                {
                    return BadRequest("User already exists");
                }

                if (userToCreate.UserType.Equals(UserType.CLIENT) || userToCreate.UserType.Equals(UserType.BEGELEIDER))
                {

                    IdentityUser identityUserToCreate = new IdentityUser()
                    {
                        Email = userToCreate.Email,
                        NormalizedEmail = userToCreate.Email,
                        UserName = userToCreate.Email,
                        NormalizedUserName = userToCreate.Email,
                        LockoutEnabled = true,
                        EmailConfirmed = true
                    };
                    await _userManager.CreateAsync(identityUserToCreate, "P@ssword1");
                }

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
        /// <summary>
        /// Edit a user
        /// </summary>
        /// <param name="model">The user</param>
        /// <returns>The user</returns>
        [HttpPut]
        public ActionResult<User> Edit(UserDTO model)
        {
            User userToEdit = _userRepository.GetById(model.UserId);
            userToEdit.UserType = model.UserType;
            userToEdit.FirstName = model.FirstName;
            userToEdit.LastName = model.LastName;
            userToEdit.Email = model.Email;
            userToEdit.ProfilePicture = model.ProfilePicture;
            userToEdit.Group = model.Group;
            userToEdit.DegreeOfLimitation = model.DegreeOfLimitation;

            if (userToEdit.UserType.Equals(UserType.CLIENT) || userToEdit.UserType.Equals(UserType.BEGELEIDER))
            {
                IdentityUser identityUserToEdit = GetUser(userToEdit.Email).GetAwaiter().GetResult();
                identityUserToEdit.Email = userToEdit.Email;
                identityUserToEdit.NormalizedEmail = userToEdit.Email;
                identityUserToEdit.UserName = userToEdit.Email;
                identityUserToEdit.NormalizedUserName = userToEdit.Email;
            }

            _userRepository.SaveChanges();
            return Ok(userToEdit);
        }

        // Works 90% off the time, sometimes produces an error: using result of async call to remove user, doesn't always happen fast enough
        // NEED TO FIX
        /// <summary>
        /// Remove a user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The user</returns>
        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult<User>> Remove(int userId)
        {
            User userToDelete = _userRepository.GetById(userId);

            if (userToDelete == null)
            {
                return NoContent();
            }
            else
            {
                if (userToDelete.UserType.Equals(UserType.CLIENT) || userToDelete.UserType.Equals(UserType.BEGELEIDER))
                {

                    IdentityUser identityUserToDelete = GetUser(userToDelete.Email).GetAwaiter().GetResult();
                    await _userManager.DeleteAsync(identityUserToDelete);
                }
                _userRepository.Remove(userToDelete);
                _userRepository.SaveChanges();
                return Ok(userToDelete);
            }
        }

        #region Private methods
        private async Task<string> GetToken(IdentityUser user)
        {
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email)
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

        private async Task<IdentityUser> GetUser(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        private async Task<bool> CheckPassword(IdentityUser identityUser, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false);
            return result.Succeeded;
        }
        #endregion
    }
}
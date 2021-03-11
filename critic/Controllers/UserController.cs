using Critic.Authorization;
using Critic.Data;
using Critic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AppUser = Critic.Models.AppUser;

namespace Critic.Controllers
{
    [Route("/api/1.0.0/users")]
    public class UserController : Controller
    {
        private CriticDbContext _context;
        private CriticBusinessService _businessService;
        public UserController(CriticDbContext context, CriticBusinessService businessService)
        {
            _context = context;
            _businessService = businessService;
        }

        #region REST methods
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        [Route("")]
        [GoogleAuthorize(AppUser.Roles.Admin)]
        public async Task<IActionResult> GetUsers([FromQuery] string name = null)
        {
            var users = await _context.Users.Where(u =>(name != null ? u.Name.Contains(name) : true)).ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get details for a specific user
        /// </summary>
        /// <param name="userId">ID of user to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpPost]
        [Route("")]
        [GoogleAuthorize]
        public async Task<IActionResult> CreateUser([FromBody] AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await GetUser(user.Id);
        }


        /// <summary>
        /// Get details for a specific user
        /// </summary>
        /// <param name="userId">ID of user to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Route("{userId}")]
        [GoogleAuthorize]
        public async Task<IActionResult> GetUser([FromRoute][Required] int? userId)
        {
            var user = await _businessService.FindUser(userId);
            if (user == null) return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Get details for a specific user
        /// </summary>
        /// <param name="email">Email of user to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Route("byEmail/{email}")]
        [GoogleAuthorize]
        public async Task<IActionResult> GetUser([FromRoute][Required] string email)
        {
            var user = await _businessService.FindUserByEmail(email);
            if (user == null) return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Edits a specific user
        /// </summary>
        /// <param name="userId">ID of user to return</param>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpPost]
        [Route("{userId}")]
        [GoogleAuthorize(AppUser.Roles.Admin)]
        public async Task<IActionResult> EditUser([FromRoute][Required] int? userId, [FromBody] AppUser newUser)
        {
            var user = await _businessService.FindUser(userId);
            if (user == null) return NotFound();

            user.Name = newUser.Name;
            user.Email = newUser.Email;
            user.Image = newUser.Image;
            user.Role = newUser.Role;
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
            return await GetUser(user.Id);
        }

        /// <summary>
        /// Removes a specific user
        /// </summary>
        /// <param name="userId">ID of user to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpDelete]
        [Route("{userId}")]
        [GoogleAuthorize(AppUser.Roles.Admin)]
        public async Task<IActionResult> DeleteUser([FromRoute][Required] int? userId)
        {
            var user = await _businessService.FindUser(userId);
            if (user == null) return NotFound();

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return Ok(new { Success = true });
        } 
        #endregion

    }
}

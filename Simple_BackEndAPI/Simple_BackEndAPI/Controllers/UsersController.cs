using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Simple_BackEndAPI.Models;
using Simple_BackEndAPI.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserContext _userContext;
        private readonly IConfiguration _config;

        public UsersController(UserContext userContext, IConfiguration config)
        {
            _userContext = userContext;
            _config = config;
        }

        //read user
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetTodoItems()
        {
            var users = await _userContext.Users.ToListAsync();
            if (users == null) { return NotFound(); }

            return Ok(users);
        }

        //read user by User_id
        [Authorize]
        [HttpGet("{User_Id}")]
        public async Task<ActionResult<User>> GetTodoItem(int User_Id)
        {
            var todoItem = await _userContext.Users.FindAsync(User_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create user
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> PostTodoItem(User newUser)
        {
            
            if (!newUser.Email.Contains("@"))
            {
                return BadRequest("email is incorrect format");
            }
#pragma warning disable CS8604 // Possible null reference argument.
            var todoItem = new User
            {
                User_Id = newUser.User_Id,
                Part_Id = newUser.Part_Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Utype_Id = newUser.Utype_Id,
                Password = Utils.Utils.Encrypt(newUser.Password, _config["Jwt:SecretKey"]),
                Enabled = newUser.Enabled
            };
#pragma warning restore CS8604 // Possible null reference argument.
            _userContext.Users.Add(todoItem);
            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _userContext.Remove(todoItem);
                if (ex.InnerException != null)
                    await Utils.Utils.AddLogsAsync(_userContext, ex.InnerException.Message);
                else
                    await Utils.Utils.AddLogsAsync(_userContext, ex.Message);
                return BadRequest(ex.Message);
            }

            var createdUser = await _userContext.Users.FindAsync(newUser.User_Id);
            return todoItem;
        }

        //update user
        [Authorize]
        [HttpPut("{User_Id}")]
        public async Task<IActionResult> PutTodoItem(int User_Id, User newUser)
        {
            if (User_Id != newUser.User_Id)
            {
                return BadRequest();
            }

            var todoItem = await _userContext.Users.FindAsync(User_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Part_Id = newUser.Part_Id;
            todoItem.FirstName = newUser.FirstName;
            todoItem.LastName = newUser.LastName;
            todoItem.Email = newUser.Email;
            todoItem.Utype_Id = newUser.Utype_Id;
#pragma warning disable CS8604 // Possible null reference argument.
            todoItem.Password = Utils.Utils.Encrypt(newUser.Password, _config["Jwt:SecretKey"]);
#pragma warning restore CS8604 // Possible null reference argument.
            todoItem.Enabled = newUser.Enabled;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(User_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete user
        [Authorize]
        [HttpDelete("{User_Id}")]
        public async Task<IActionResult> DeleteTodoItem(int User_Id)
        {
            var todoItem = await _userContext.Users.FindAsync(User_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Users.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }

        private static User ItemToDTO(User newUser) =>
        new User
        {
            User_Id = newUser.User_Id,
            Part_Id = newUser.Part_Id,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Utype_Id = newUser.Utype_Id,
            Password = newUser.Password,
            Enabled = newUser.Enabled
        };

        private bool TodoItemExists(int User_Id)
        {
            return _userContext.Users.Any(e => e.User_Id == User_Id);
        }
    }
}

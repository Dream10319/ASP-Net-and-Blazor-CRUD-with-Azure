using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersTypeController : ControllerBase
    {
        private UserContext _userContext;

        public UsersTypeController(UserContext userContext)
        {
            _userContext = userContext;

        }

        //read users_Type
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Users_Type>> GetTodoItems()
        {
            var users_Types = await _userContext.Users_Types.ToListAsync();
            if (users_Types == null) { return NotFound(); }

            return Ok(users_Types);
        }

        //read user by Utipo_Id
        [Authorize]
        [HttpGet("{Utipo_Id}")]
        public async Task<ActionResult<Users_Type>> GetTodoItem(int Utipo_Id)
        {
            var todoItem = await _userContext.Users_Types.FindAsync(Utipo_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create users_Type
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Users_Type>> PostTodoItem(Users_Type newUserType)
        {
            var todoItem = new Users_Type
            {
                Utipo_Id = newUserType.Utipo_Id,
                Description = newUserType.Description
            };
            _userContext.Users_Types.Add(todoItem);
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

            var createdUserType = await _userContext.Users_Types.FindAsync(newUserType.Utipo_Id);
            return todoItem;
        }

        //update users_Type
        [Authorize]
        [HttpPut("{Utipo_Id}")]
        public async Task<IActionResult> PutTodoItem(int Utipo_Id, Users_Type newUserType)
        {
            if (Utipo_Id != newUserType.Utipo_Id)
            {
                return BadRequest();
            }

            var todoItem = await _userContext.Users_Types.FindAsync(Utipo_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Utipo_Id = newUserType.Utipo_Id;
            todoItem.Description = newUserType.Description;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(Utipo_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete user
        [Authorize]
        [HttpDelete("{Utipo_Id}")]
        public async Task<IActionResult> DeleteTodoItem(int Utipo_Id)
        {
            var todoItem = await _userContext.Users_Types.FindAsync(Utipo_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Users_Types.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }

        private static Users_Type ItemToDTO(Users_Type newUserType) =>
        new Users_Type
        {
            Utipo_Id = newUserType.Utipo_Id,
            Description = newUserType.Description
        };

        private bool TodoItemExists(int Utipo_Id)
        {
            return _userContext.Users_Types.Any(e => e.Utipo_Id == Utipo_Id);
        }
    }
}

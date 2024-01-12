using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTypeController : ControllerBase
    {
        private UserContext _userContext;

        public NotificationTypeController(UserContext userContext)
        {
            _userContext = userContext;

        }

        //read Notification_Type
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Notification_Type>> GetTodoItems()
        {
            var Notification_Types = await _userContext.Notification_Types.ToListAsync();
            if (Notification_Types == null) { return NotFound(); }

            return Ok(Notification_Types);
        }

        //read Notification_Type by Notification_Id
        [Authorize]
        [HttpGet("{Notification_Id}")]
        public async Task<ActionResult<Notification_Type>> GetTodoItem(int Notification_Id)
        {
            var todoItem = await _userContext.Notification_Types.FindAsync(Notification_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create Notification_Type
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Notification_Type>> PostTodoItem(Notification_Type newNotificationType)
        {
            var todoItem = new Notification_Type
            {
                Notification_Id = newNotificationType.Notification_Id,
                Description = newNotificationType.Description
            };
            _userContext.Notification_Types.Add(todoItem);
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

            var createdNotificationType = await _userContext.Notification_Types.FindAsync(newNotificationType.Notification_Id);
            return todoItem;
        }

        //update Notification_Type
        [Authorize]
        [HttpPut("{Notification_Id}")]
        public async Task<IActionResult> PutTodoItem(int Notification_Id, Notification_Type newNotificationType)
        {
            if (Notification_Id != newNotificationType.Notification_Id)
            {
                return BadRequest();
            }

            var todoItem = await _userContext.Notification_Types.FindAsync(Notification_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Notification_Id = newNotificationType.Notification_Id;
            todoItem.Description = newNotificationType.Description;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(Notification_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete Notification_Type
        [Authorize]
        [HttpDelete("{Notification_Id}")]
        public async Task<IActionResult> DeleteTodoItem(int Notification_Id)
        {
            var todoItem = await _userContext.Notification_Types.FindAsync(Notification_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Notification_Types.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }

        private static Notification_Type ItemToDTO(Notification_Type newNotificationType) =>
        new Notification_Type
        {
            Notification_Id = newNotificationType.Notification_Id,
            Description = newNotificationType.Description
        };

        private bool TodoItemExists(int Notification_Id)
        {
            return _userContext.Notification_Types.Any(e => e.Notification_Id == Notification_Id);
        }
    }
}

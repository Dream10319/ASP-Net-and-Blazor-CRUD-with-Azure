using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private UserContext _userContext;

        public LogsController(UserContext userContext)
        {
            _userContext = userContext;

        }

        //read Logs
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Log>> GetTodoItems()
        {
            var Logs = await _userContext.Logs.ToListAsync();
            if (Logs == null) { return NotFound(); }

            return Ok(Logs);
        }

        //read log by Log_Id
        [Authorize]
        [HttpGet("{Log_Id}")]
        public async Task<ActionResult<Log>> GetTodoItem(long Log_Id)
        {
            var todoItem = await _userContext.Logs.FindAsync(Log_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //delete user
        [Authorize]
        [HttpDelete("{Log_Id}")]
        public async Task<IActionResult> DeleteTodoItem(long Log_Id)
        {
            var todoItem = await _userContext.Logs.FindAsync(Log_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Logs.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }
    }
}

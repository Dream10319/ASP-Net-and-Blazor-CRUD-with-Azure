using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificationTypeController : ControllerBase
    {
        private UserContext _userContext;

        public IdentificationTypeController(UserContext userContext)
        {
            _userContext = userContext;

        }

        //read Identification_Type
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Identification_Type>> GetTodoItems()
        {
            var Identification_Types = await _userContext.Identification_Types.ToListAsync();
            if (Identification_Types == null) { return NotFound(); }

            return Ok(Identification_Types);
        }

        //read Identification_Type by Identification_Id
        [Authorize]
        [HttpGet("{Identification_Id}")]
        public async Task<ActionResult<Identification_Type>> GetTodoItem(int Identification_Id)
        {
            var todoItem = await _userContext.Identification_Types.FindAsync(Identification_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create Identification_Type
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Identification_Type>> PostTodoItem(Identification_Type newIdentificationType)
        {
            var todoItem = new Identification_Type
            {
                Identification_Id = newIdentificationType.Identification_Id,
                Identification_type1 = newIdentificationType.Identification_type1,
                Identification_Format = newIdentificationType.Identification_Format
            };
            _userContext.Identification_Types.Add(todoItem);
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

            var createdIdentificationType = await _userContext.Identification_Types.FindAsync(newIdentificationType.Identification_Id);
            return todoItem;
        }

        //update Identification_Type
        [Authorize]
        [HttpPut("{Identification_Id}")]
        public async Task<IActionResult> PutTodoItem(int Identification_Id, Identification_Type newIdentificationType)
        {
            if (Identification_Id != newIdentificationType.Identification_Id)
            {
                return BadRequest();
            }

            var todoItem = await _userContext.Identification_Types.FindAsync(Identification_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Identification_Id = newIdentificationType.Identification_Id;
            todoItem.Identification_type1 = newIdentificationType.Identification_type1;
            todoItem.Identification_Format = newIdentificationType.Identification_Format;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(Identification_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete Identification_Type
        [Authorize]
        [HttpDelete("{Identification_Id}")]
        public async Task<IActionResult> DeleteTodoItem(int Identification_Id)
        {
            var todoItem = await _userContext.Identification_Types.FindAsync(Identification_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Identification_Types.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }

        private static Identification_Type ItemToDTO(Identification_Type newIdentificationType) =>
        new Identification_Type
        {
            Identification_Id = newIdentificationType.Identification_Id,
            Identification_type1 = newIdentificationType.Identification_type1,
            Identification_Format = newIdentificationType.Identification_Format
        };

        private bool TodoItemExists(int Identification_Id)
        {
            return _userContext.Identification_Types.Any(e => e.Identification_Id == Identification_Id);
        }
    }
}

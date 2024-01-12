using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsTypeController : ControllerBase
    {
        private UserContext _userContext;

        public ParticipantsTypeController(UserContext userContext)
        {
            _userContext = userContext;

        }

        //read Participants_Type
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Participants_Type>> GetTodoItems()
        {
            var Participants_Types = await _userContext.Participants_Types.ToListAsync();
            if (Participants_Types == null) { return NotFound(); }

            return Ok(Participants_Types);
        }

        //read Participants_Type by Ptype_Id
        [Authorize]
        [HttpGet("{Ptype_Id}")]
        public async Task<ActionResult<Participants_Type>> GetTodoItem(int Ptype_Id)
        {
            var todoItem = await _userContext.Participants_Types.FindAsync(Ptype_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create Participants_Type
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Participants_Type>> PostTodoItem(Participants_Type newParticipantType)
        {
            var todoItem = new Participants_Type
            {
                Ptype_Id = newParticipantType.Ptype_Id,
                ShortName = newParticipantType.ShortName,
                Description = newParticipantType.Description
            };
            _userContext.Participants_Types.Add(todoItem);
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

            var createdParticipantType = await _userContext.Participants_Types.FindAsync(newParticipantType.Ptype_Id);
            return todoItem;
        }

        //update Participants_Type
        [Authorize]
        [HttpPut("{Ptype_Id}")]
        public async Task<IActionResult> PutTodoItem(int Ptype_Id, Participants_Type newParticipantType)
        {
            if (Ptype_Id != newParticipantType.Ptype_Id)
            {
                return BadRequest();
            }

            var todoItem = await _userContext.Participants_Types.FindAsync(Ptype_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Ptype_Id = newParticipantType.Ptype_Id;
            todoItem.ShortName = newParticipantType.ShortName;
            todoItem.Description = newParticipantType.Description;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(Ptype_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete Participants_Type
        [Authorize]
        [HttpDelete("{Ptype_Id}")]
        public async Task<IActionResult> DeleteTodoItem(int Ptype_Id)
        {
            var todoItem = await _userContext.Participants_Types.FindAsync(Ptype_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Participants_Types.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }

        private static Participants_Type ItemToDTO(Participants_Type newParticipantType) =>
        new Participants_Type
        {
            Ptype_Id = newParticipantType.Ptype_Id,
            ShortName = newParticipantType.ShortName,
            Description = newParticipantType.Description
        };

        private bool TodoItemExists(int Ptype_Id)
        {
            return _userContext.Participants_Types.Any(e => e.Ptype_Id == Ptype_Id);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private UserContext _userContext;

        public ParticipantsController(UserContext userContext)
        {
            _userContext = userContext;

        }

        //read Participants
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Participant>> GetTodoItems()
        {
            var Participants = await _userContext.Participants.ToListAsync();
            if (Participants == null) { return NotFound(); }

            return Ok(Participants);
        }

        //read Participants by Part_Id
        [Authorize]
        [HttpGet("{Part_Id}")]
        public async Task<ActionResult<Participant>> GetTodoItem(int Part_Id)
        {
            var todoItem = await _userContext.Participants.FindAsync(Part_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create Participant
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Participant>> PostTodoItem(Participant newParticipants)
        {
            var todoItem = new Participant
            {
                Part_Id = newParticipants.Part_Id,
                Ptype_Id = newParticipants.Ptype_Id,
                Country_Id = newParticipants.Country_Id,
                Description = newParticipants.Description,
                Enabled = newParticipants.Enabled
            };
            _userContext.Participants.Add(todoItem);
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

            var createdParticipant = await _userContext.Participants.FindAsync(newParticipants.Part_Id);
            return todoItem;
        }

        //update Participant
        [Authorize]
        [HttpPut("{Part_Id}")]
        public async Task<IActionResult> PutTodoItem(int Part_Id, Participant newParticipant)
        {
            if (Part_Id != newParticipant.Part_Id)
            {
                return BadRequest();
            }

            var todoItem = await _userContext.Participants.FindAsync(Part_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Part_Id = newParticipant.Part_Id;
            todoItem.Ptype_Id = newParticipant.Ptype_Id;
            todoItem.Country_Id = newParticipant.Country_Id;
            todoItem.Description = newParticipant.Description;
            todoItem.Enabled = newParticipant.Enabled;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(Part_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete user
        [Authorize]
        [HttpDelete("{Part_Id}")]
        public async Task<IActionResult> DeleteTodoItem(int Part_Id)
        {
            var todoItem = await _userContext.Participants.FindAsync(Part_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Participants.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }

        private static Participant ItemToDTO(Participant newParticipant) =>
        new Participant
        {
            Part_Id = newParticipant.Part_Id,
            Ptype_Id = newParticipant.Ptype_Id,
            Country_Id = newParticipant.Country_Id,
            Description = newParticipant.Description,
            Enabled = newParticipant.Enabled
        };

        private bool TodoItemExists(int Part_Id)
        {
            return _userContext.Participants.Any(e => e.Part_Id == Part_Id);
        }
    }
}

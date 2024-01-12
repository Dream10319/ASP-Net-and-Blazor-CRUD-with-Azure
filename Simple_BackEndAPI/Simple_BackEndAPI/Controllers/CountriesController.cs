using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private UserContext _userContext;

        public CountriesController(UserContext userContext)
        {
            _userContext = userContext;

        }

        //read Country
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Country>> GetTodoItems()
        {
            var Countries = await _userContext.Countries.ToListAsync();
            if (Countries == null) { return NotFound(); }

            return Ok(Countries);
        }

        //read country by Country_Id
        [Authorize]
        [HttpGet("{Country_Id}")]
        public async Task<ActionResult<Country>> GetTodoItem(int Country_Id)
        {
            var todoItem = await _userContext.Countries.FindAsync(Country_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create Country
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Country>> PostTodoItem(Country newCountry)
        {
            var todoItem = new Country
            {
                Country_Id = newCountry.Country_Id,
                Description = newCountry.Description
            };

            _userContext.Countries.Add(todoItem);
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

            var createdCountry = await _userContext.Countries.FindAsync(newCountry.Country_Id);
            return todoItem;
        }

        //update Country
        [Authorize]
        [HttpPut("{Country_Id}")]
        public async Task<IActionResult> PutTodoItem(int Country_Id, Country newCountry)
        {
            if (Country_Id != newCountry.Country_Id)
            {
                return BadRequest();
            }

            var todoItem = await _userContext.Countries.FindAsync(Country_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Country_Id = newCountry.Country_Id;
            todoItem.Description = newCountry.Description;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(Country_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete user
        [Authorize]
        [HttpDelete("{Country_Id}")]
        public async Task<IActionResult> DeleteTodoItem(int Country_Id)
        {
            var todoItem = await _userContext.Countries.FindAsync(Country_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Countries.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }

        private static Country ItemToDTO(Country newCountry) =>
        new Country
        {
            Country_Id = newCountry.Country_Id,
            Description = newCountry.Description
        };

        private bool TodoItemExists(int Country_Id)
        {
            return _userContext.Countries.Any(e => e.Country_Id == Country_Id);
        }
    }
}

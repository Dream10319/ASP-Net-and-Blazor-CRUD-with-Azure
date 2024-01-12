using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private UserContext _userContext;

        public PaymentTypeController(UserContext userContext)
        {
            _userContext = userContext;

        }

        //read Payment_Type
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Payment_Type>> GetTodoItems()
        {
            var Payment_Types = await _userContext.Payment_Types.ToListAsync();
            if (Payment_Types == null) { return NotFound(); }

            return Ok(Payment_Types);
        }

        //read Payment_Type by Payment_Id
        [Authorize]
        [HttpGet("{Payment_Id}")]
        public async Task<ActionResult<Payment_Type>> GetTodoItem(int Payment_Id)
        {
            var todoItem = await _userContext.Payment_Types.FindAsync(Payment_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create Payment_Type
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Payment_Type>> PostTodoItem(Payment_Type newPaymentType)
        {
            var todoItem = new Payment_Type
            {
                Payment_Id = newPaymentType.Payment_Id,
                Description = newPaymentType.Description
            };
            _userContext.Payment_Types.Add(todoItem);
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

            var createdPaymentType = await _userContext.Payment_Types.FindAsync(newPaymentType.Payment_Id);
            return todoItem;
        }

        //update Payment_Type
        [Authorize]
        [HttpPut("{Payment_Id}")]
        public async Task<IActionResult> PutTodoItem(int Payment_Id, Payment_Type newPaymentType)
        {
            if (Payment_Id != newPaymentType.Payment_Id)
            {
                return BadRequest();
            }

            var todoItem = await _userContext.Payment_Types.FindAsync(Payment_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Payment_Id = newPaymentType.Payment_Id;
            todoItem.Description = newPaymentType.Description;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(Payment_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete Payment_Type
        [Authorize]
        [HttpDelete("{Payment_Id}")]
        public async Task<IActionResult> DeleteTodoItem(int Payment_Id)
        {
            var todoItem = await _userContext.Payment_Types.FindAsync(Payment_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _userContext.Payment_Types.Remove(todoItem);
            await _userContext.SaveChangesAsync();

            return Ok();
        }

        private static Payment_Type ItemToDTO(Payment_Type newPaymentType) =>
        new Payment_Type
        {
            Payment_Id = newPaymentType.Payment_Id,
            Description = newPaymentType.Description
        };

        private bool TodoItemExists(int Payment_Id)
        {
            return _userContext.Payment_Types.Any(e => e.Payment_Id == Payment_Id);
        }
    }
}

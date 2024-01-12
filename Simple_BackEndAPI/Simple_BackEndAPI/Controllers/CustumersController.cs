using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_BackEndAPI.Models;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private UserContext _UserContext;

        public CustomersController(UserContext UserContext)
        {
            _UserContext = UserContext;

        }

        //read Customer
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Customer>> GetTodoItems()
        {
            var Customers = await _UserContext.Customers.ToListAsync();
            if (Customers == null) { return NotFound(); }

            return Ok(Customers);
        }

        //read Customer by Cliente_Id
        [Authorize]
        [HttpGet("{Cliente_Id}")]
        public async Task<ActionResult<Customer>> GetTodoItem(long Cliente_Id)
        {
            var todoItem = await _UserContext.Customers.FindAsync(Cliente_Id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //create Customer
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Customer>> PostTodoItem(Customer newCustomer)
        {
            if (!newCustomer.Email.Contains("@"))
            {
                return BadRequest("email is incorrect format");
            }
            var todoItem = new Customer
            {
                Cliente_Id = newCustomer.Cliente_Id,
                Part_Id = newCustomer.Part_Id,
                FirstName = newCustomer.FirstName,
                MiddleName = newCustomer.MiddleName,
                LastNames = newCustomer.LastNames,
                Identification_Id = newCustomer.Identification_Id,
                Identification_Number = newCustomer.Identification_Number,
                CountryCode = newCustomer.CountryCode,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email,
                CreateDate = newCustomer.CreateDate,
                UpdateDate = newCustomer.UpdateDate,
                Last_User_Id_Change = newCustomer.Last_User_Id_Change
            };
            _UserContext.Customers.Add(todoItem);
            try
            {
                await _UserContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _UserContext.Remove(todoItem);
                if (ex.InnerException != null)
                    await Utils.Utils.AddLogsAsync(_UserContext, ex.InnerException.Message);
                else
                    await Utils.Utils.AddLogsAsync(_UserContext, ex.Message);
                return BadRequest(ex.Message);
            }

            var createdCustomer = await _UserContext.Customers.FindAsync(newCustomer.Cliente_Id);
            return todoItem;
        }

        //update Customer
        [Authorize]
        [HttpPut("{Cliente_Id}")]
        public async Task<IActionResult> PutTodoItem(long Cliente_Id, Customer newCustomer)
        {
            if (Cliente_Id != newCustomer.Cliente_Id)
            {
                return BadRequest();
            }

            var todoItem = await _UserContext.Customers.FindAsync(Cliente_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Part_Id = newCustomer.Part_Id;
            todoItem.FirstName = newCustomer.FirstName;
            todoItem.MiddleName = newCustomer.MiddleName;
            todoItem.LastNames = newCustomer.LastNames;
            todoItem.Identification_Id = newCustomer.Identification_Id;
            todoItem.Identification_Number = newCustomer.Identification_Number;
            todoItem.CountryCode = newCustomer.CountryCode;
            todoItem.PhoneNumber = newCustomer.PhoneNumber;
            todoItem.Email = newCustomer.Email;
            todoItem.CreateDate = newCustomer.CreateDate;
            todoItem.UpdateDate = newCustomer.UpdateDate;
            todoItem.Last_User_Id_Change = newCustomer.Last_User_Id_Change;

            try
            {
                await _UserContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(Cliente_Id))
            {
                return NotFound();
            }

            return Ok();
        }

        //delete Customer
        [Authorize]
        [HttpDelete("{Cliente_Id}")]
        public async Task<IActionResult> DeleteTodoItem(long Cliente_Id)
        {
            var todoItem = await _UserContext.Customers.FindAsync(Cliente_Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _UserContext.Customers.Remove(todoItem);
            await _UserContext.SaveChangesAsync();

            return Ok();
        }

        private static Customer ItemToDTO(Customer newCustomer) =>
        new Customer
        {
            Cliente_Id = newCustomer.Cliente_Id,
            Part_Id = newCustomer.Part_Id,
            FirstName = newCustomer.FirstName,
            MiddleName = newCustomer.MiddleName,
            LastNames = newCustomer.LastNames,
            Identification_Id = newCustomer.Identification_Id,
            Identification_Number = newCustomer.Identification_Number,
            CountryCode = newCustomer.CountryCode,
            PhoneNumber = newCustomer.PhoneNumber,
            Email = newCustomer.Email,
            CreateDate = newCustomer.CreateDate,
            UpdateDate = newCustomer.UpdateDate,
            Last_User_Id_Change = newCustomer.Last_User_Id_Change
        };

        private bool TodoItemExists(long Cliente_Id)
        {
            return _UserContext.Customers.Any(e => e.Cliente_Id == Cliente_Id);
        }
    }
}

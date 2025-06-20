using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using OrderServices.Helpers;
using OrderServices.Models;
using OrderServices.Services;
using OrderServices.ViewModels;

namespace OrderServices.Controllers
{
    [Route("api/customers")]
    [ValidateModel]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = _customerService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequest customer)
        {
            var result = await _customerService.AddAsync(customer);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerRequest customer)
        {
            var result = await _customerService.UpdateAsync(id, customer);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}

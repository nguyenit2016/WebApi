using Microsoft.AspNetCore.Mvc;
using OrderServices.Models;
using OrderServices.Services;

namespace OrderServices.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("api/customers")]
        public async Task<IActionResult> GetAll()
        {
            var customers = _customerService.GetAll();
            return Ok(customers);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OrderServices.Helpers;
using OrderServices.Models;
using OrderServices.Services;
using OrderServices.ViewModels;

namespace OrderServices.Controllers
{
    [Route("api/orders")]
    [ValidateModel]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderFilterRequest filter)
        {
            var result = await _orderService.GetAll(filter);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest order)
        {
            var result = await _orderService.CreateAsync(order);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
    }
}

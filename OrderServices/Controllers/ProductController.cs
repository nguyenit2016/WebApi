using Microsoft.AspNetCore.Mvc;
using OrderServices.Helpers;
using OrderServices.Services;
using OrderServices.ViewModels;

namespace OrderServices.Controllers
{
    [Route("api/products")]
    [ValidateModel]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = _productService.GetAll();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest product)
        {
            var result = await _productService.AddAsync(product);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest product)
        {
            var result = await _productService.UpdateAsync(id, product);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
    }
}

using CrudApp.Models;
using CrudApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CrudApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            await _productRepository.AddAsync(productDto);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            await _productRepository.UpdateAsync(productDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

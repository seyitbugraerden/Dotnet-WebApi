using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webapi.DTO;

namespace shopapp.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAll();
            var productsDTO = new List<ProductDTO>();
            foreach (var product in products)
            {
                productsDTO.Add(new ProductDTO
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Url = product.Url,
                    Price = product.Price,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl
                });
            }
            return Ok(productsDTO);
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.GetById(id);
            var productDTO = new ProductDTO
            {
                ProductId = product.Result.ProductId,
                Name = product.Result.Name,
                Url = product.Result.Url,
                Price = product.Result.Price,
                Description = product.Result.Description,
                ImageUrl = product.Result.ImageUrl
            };
            if (product == null)
                return NotFound();
            return Ok(productDTO);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            await _productService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetProduct), new { id = entity.ProductId }, entity);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product entity)
        {
            if(id != entity.ProductId)
                return BadRequest();
            var product = await _productService.GetById(id);
            if(product == null)
                return NotFound();
            await _productService.UpdateAsync(product,entity);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetById(id);
            if(product == null)
                return NotFound();
            await _productService.DeleteAsync(product);
            return NoContent();
        }
    }
}

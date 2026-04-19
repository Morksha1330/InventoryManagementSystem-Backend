using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgtSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<HttpResponseData<ProductDTO>>> GetAll()
        {
            return Ok(await _service.GetAllProducts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HttpResponseData<ProductDTO>>> GetById(int id)
        {
            return Ok(await _service.GetProductById(id));
        }

        [HttpPost]
        public async Task<ActionResult<HttpResponseData<ProductDTO>>> Create(ProductDTO dto)
        {
            return Ok(await _service.AddProduct(dto));
        }

        [HttpPut]
        public async Task<ActionResult<HttpResponseData<ProductDTO>>> Update(ProductDTO dto)
        {
            return Ok(await _service.UpdateProduct(dto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpResponseData<ProductDTO>>> Delete(int id)
        {
            return Ok(await _service.DeleteProduct(id));
        }
    }
}
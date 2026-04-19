using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Services;
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

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProducts([FromQuery] RequestFilterDto filter)
        {
            var response = await _service.GetPagedProductsAsync(filter);

            if (!response.Success)
            {
                return StatusCode(response.ResponsCode, response);
            }

            var result = new
            {
                response.Success,
                response.ResponsCode,
                response.Message,
                Data = response.Result.Items,
                Pagination = new
                {
                    response.Result.TotalCount,
                    response.Result.PageNumber,
                    response.Result.PageSize,
                    response.Result.TotalPages,
                    response.Result.HasPreviousPage,
                    response.Result.HasNextPage
                }
            };

            return Ok(result);
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
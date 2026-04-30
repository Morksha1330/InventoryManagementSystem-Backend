using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgtSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCategories([FromQuery] RequestFilterDto filter)
        {
            var response = await _service.GetPagedCategoriesAsync(filter);

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
        public async Task<ActionResult<HttpResponseData<CategoryDTO>>> GetById(int id)
        {
            return Ok(await _service.GetCategoryById(id));
        }

        [HttpPost]
        public async Task<ActionResult<HttpResponseData<CategoryDTO>>> Create(CategoryDTO dto)
        {
            return Ok(await _service.AddCategory(dto));
        }

        [HttpPut]
        public async Task<ActionResult<HttpResponseData<CategoryDTO>>> Update(CategoryDTO dto)
        {
            return Ok(await _service.UpdateCategory(dto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpResponseData<CategoryDTO>>> Delete(int id)
        {
            return Ok(await _service.DeleteCategory(id));
        }
    }
}
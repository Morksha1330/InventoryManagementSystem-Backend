using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgtSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCustomers([FromQuery] RequestFilterDto filter)
        {
            var response = await _customerService.GetPagedCustomersAsync(filter);

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
        public async Task<IActionResult> GetCustomer(int id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);

            return StatusCode(response.ResponsCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new HttpResponseData<Customer>
                {
                    Success = false,
                    ResponsCode = 400,
                    Error = "Validation failed",
                    Message = "Please check the submitted data"
                };

                return BadRequest(errorResponse);
            }

            var response = await _customerService.CreateCustomerAsync(customer);

            return StatusCode(response.ResponsCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
            {
                var errorResponse = new HttpResponseData<Customer>
                {
                    Success = false,
                    ResponsCode = 400,
                    Error = "ID mismatch",
                    Message = "Customer ID in URL does not match request body"
                };

                return BadRequest(errorResponse);
            }

            if (!ModelState.IsValid)
            {
                var errorResponse = new HttpResponseData<Customer>
                {
                    Success = false,
                    ResponsCode = 400,
                    Error = "Validation failed",
                    Message = "Please check the submitted data"
                };

                return BadRequest(errorResponse);
            }

            var response = await _customerService.UpdateCustomerAsync(customer);

            return StatusCode(response.ResponsCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var response = await _customerService.DeleteCustomerAsync(id);

            return StatusCode(response.ResponsCode, response);
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleCustomerStatus(int id)
        {
            var response = await _customerService.ToggleCustomerStatusAsync(id);

            return StatusCode(response.ResponsCode, response);
        }
    }
}
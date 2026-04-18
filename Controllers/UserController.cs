using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgtSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    { 
        _userService = userService;
    }

        /// <summary>
        /// Get paginated users with search filters
        /// </summary>
        /// <param name="filter">Filter parameters including search term, pagination, and sorting</param>
        /// <returns>Paginated list of users with role names</returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedUsers([FromQuery] UserFilterDto filter)
        {
            var response = await _userService.GetPagedUsersAsync(filter);
            
            if (!response.Success)
            {
                return StatusCode(response.ResponsCode, response);
            }
            
            // Create a wrapper that includes pagination metadata
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

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return StatusCode(response.ResponsCode, response);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new HttpResponseData<User>
                {
                    Success = false,
                    ResponsCode = 400,
                    Error = "Validation failed",
                    Message = "Please check the submitted data"
                };
                return BadRequest(errorResponse);
            }

            var response = await _userService.CreateUserAsync(user);
            return StatusCode(response.ResponsCode, response);
        }

        /// <summary>
        /// Update existing user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto user)
        {
            if (id != user.Id)
            {
                var errorResponse = new HttpResponseData<User>
                {
                    Success = false,
                    ResponsCode = 400,
                    Error = "ID mismatch",
                    Message = "User ID in URL does not match the ID in the request body"
                };
                return BadRequest(errorResponse);
            }

            if (!ModelState.IsValid)
            {
                var errorResponse = new HttpResponseData<User>
                {
                    Success = false,
                    ResponsCode = 400,
                    Error = "Validation failed",
                    Message = "Please check the submitted data"
                };
                return BadRequest(errorResponse);
            }

            var response = await _userService.UpdateUserAsync(user);
            return StatusCode(response.ResponsCode, response);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUserAsync(id);
            return StatusCode(response.ResponsCode, response);
        }

        /// <summary>
        /// Toggle user active status
        /// </summary>
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var response = await _userService.ToggleUserStatusAsync(id);
            return StatusCode(response.ResponsCode, response);
        }
}
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryMgtSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

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


    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var response = await _userService.GetUserByIdAsync(id);
        return StatusCode(response.ResponsCode, response);
    }


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


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var response = await _userService.DeleteUserAsync(id);
        return StatusCode(response.ResponsCode, response);
    }


    [HttpPatch("{id}/toggle-status")]
    public async Task<IActionResult> ToggleUserStatus(int id)
    {
        var response = await _userService.ToggleUserStatusAsync(id);
        return StatusCode(response.ResponsCode, response);
    }


    [HttpGet("profile")]
    public async Task<IActionResult> GetLoggedInUserProfile()
    {
        var userIdClaim = Convert.ToInt32(User.FindFirst("id")?.Value);


        if (userIdClaim == null)
        {
            var errorResponse = new HttpResponseData<UserDto>
            {
                Success = false,
                ResponsCode = 401,
                Error = "Unauthorized",
                Message = "User is not authenticated"
            };
            return Unauthorized(errorResponse);
        }


        var response = await _userService.GetLoggedInUserProfileAsync(userIdClaim);
        return StatusCode(response.ResponsCode, response);
    }

    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword dto)
    {
        var userId1 = Convert.ToInt32(User.FindFirst("id")?.Value);
        //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userId1 == 0)
        {
            return Unauthorized(new HttpResponseData<bool>
            {
                Success = false,    
                ResponsCode = 401,
                Message = "Unauthorized"
            });
        }

        //int userId = int.Parse(userIdClaim.Value);

        var response = await _userService.ChangePasswordAsync(userId1, dto);
        return StatusCode(response.ResponsCode, response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest(new HttpResponseData<bool>
            {
                Success = false,
                ResponsCode = 400,
                Message = "Invalid token"
            });
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        var response = await _userService.LogoutAsync(token);
        return StatusCode(response.ResponsCode, response);
    }
}
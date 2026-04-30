using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Services.Interface;

public interface IUserService
{
    Task<HttpResponseData<UserDto>> GetUserByIdAsync(int id);
    Task<HttpResponseData<PagedResultDto<UserDto>>> GetPagedUsersAsync(UserFilterDto filter);
    Task<HttpResponseData<User>> CreateUserAsync(User user);
    Task<HttpResponseData<User>> UpdateUserAsync(UpdateUserDto user);
    Task<HttpResponseData<bool>> DeleteUserAsync(int id);
    Task<HttpResponseData<bool>> ToggleUserStatusAsync(int id);
    Task<HttpResponseData<UserDto>> GetLoggedInUserProfileAsync(int userId);
    Task<HttpResponseData<bool>> ChangePasswordAsync(int userId, ChangePassword dto);
    Task<HttpResponseData<bool>> LogoutAsync(string token);
}
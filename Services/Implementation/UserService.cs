using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using InventoryMgtSystem.Services.Interface;
using BCrypt.Net;

namespace InventoryMgtSystem.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRolesRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRolesRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<HttpResponseData<UserDto>> GetUserByIdAsync(int id)
    {
        var response = new HttpResponseData<UserDto>();

        try
        {
            var user = await _userRepository.GetUser(id);

            response.Result = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                EPF_No = user.EPF_No,
                RoleId = user.RoleId,
                RoleName = user.Role?.RoleName ?? string.Empty,
                Active = user.Active,
                InitialAttempt = user.InitialAttempt,
                CreatedUser = user.CreatedUser,
                CreatedDate = user.CreatedDate
            };

            response.Success = true;
            response.ResponsCode = 200;
            response.Message = "User retrieved successfully";
        }
        catch (KeyNotFoundException ex)
        {
            response.Success = false;
            response.ResponsCode = 404;
            response.Error = ex.Message;
            response.Message = "User not found";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred while retrieving the user";
        }

        return response;
    }

    public async Task<HttpResponseData<PagedResultDto<UserDto>>> GetPagedUsersAsync(UserFilterDto filter)
    {
        var response = new HttpResponseData<PagedResultDto<UserDto>>();

        try
        {
            // Validate and sanitize inputs
            filter.PageNumber = Math.Max(1, filter.PageNumber);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);
            filter.SortBy = string.IsNullOrWhiteSpace(filter.SortBy) ? "Id" : filter.SortBy;
            filter.SortOrder = filter.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

            var result = await _userRepository.GetPagedUsersAsync(filter);

            response.Result = result;
            response.Success = true;
            response.ResponsCode = 200;
            response.Message = "Users retrieved successfully";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred while retrieving users";
        }

        return response;
    }

    public async Task<HttpResponseData<User>> CreateUserAsync(User user)
    {
        var response = new HttpResponseData<User>();

        try
        {
            // Check if user already exists
            var exists = await _userRepository.UserExistsAsync(user.Username, user.Email);
            if (exists)
            {
                response.Success = false;
                response.ResponsCode = 409;
                response.Error = "User already exists";
                response.Message = "User with same username or email already exists";
                return response;
            }

            // Validate role exists
            if (user.RoleId > 0)
            {
                var role = await _roleRepository.GetData(user.RoleId);
                if (role == null)
                {
                    response.Success = false;
                    response.ResponsCode = 400;
                    response.Error = "Invalid Role";
                    response.Message = "The specified Role ID does not exist";
                    return response;
                }
            }

            user.CreatedDate = DateTime.UtcNow;
            user.Active = true;
            user.InitialAttempt = 0;

            _userRepository.Add(user);
            await _userRepository.Save();

            response.Result = user;
            response.Success = true;
            response.ResponsCode = 201;
            response.Message = "User created successfully";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred while creating the user";
        }

        return response;
    }

    public async Task<HttpResponseData<User>> UpdateUserAsync(UpdateUserDto user)
    {
        var response = new HttpResponseData<User>();

        try
        {
            var existingUser = await _userRepository.GetUser(user.Id);
            if (existingUser == null)
            {
                response.Success = false;
                response.ResponsCode = 404;
                response.Error = "User not found";
                response.Message = "The specified user does not exist";
                return response;
            }

            // Check if username or email is taken by another user
            var exists = await _userRepository.UserExistsAsync(user.Username, user.Email, user.Id);
            if (exists)
            {
                response.Success = false;
                response.ResponsCode = 409;
                response.Error = "Duplicate user";
                response.Message = "Username or email already taken by another user";
                return response;
            }

            // Update properties
            if (!string.IsNullOrWhiteSpace(user.Name))
                existingUser.Name = user.Name;

            if (!string.IsNullOrWhiteSpace(user.Username))
                existingUser.Username = user.Username;

            if (!string.IsNullOrWhiteSpace(user.Email))
                existingUser.Email = user.Email;

            if (!string.IsNullOrWhiteSpace(user.EPF_No))
                existingUser.EPF_No = user.EPF_No;

            if (user.RoleId.HasValue)
                existingUser.RoleId = user.RoleId.Value;

            if (user.Active.HasValue)
                existingUser.Active = user.Active.Value;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = user.Password; // Hash password in controller
            }

            _userRepository.Update(existingUser);
            await _userRepository.Save();

            response.Result = existingUser;
            response.Success = true;
            response.ResponsCode = 200;
            response.Message = "User updated successfully";
        }
        catch (KeyNotFoundException)
        {
            response.Success = false;
            response.ResponsCode = 404;
            response.Error = "User not found";
            response.Message = "The specified user does not exist";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred while updating the user";
        }

        return response;
    }

    public async Task<HttpResponseData<bool>> DeleteUserAsync(int id)
    {
        var response = new HttpResponseData<bool>();

        try
        {
            var user = await _userRepository.GetUser(id);
            if (user == null)
            {
                response.Success = false;
                response.ResponsCode = 404;
                response.Error = "User not found";
                response.Message = "The specified user does not exist";
                return response;
            }

            _userRepository.Delete(id);
            var result = await _userRepository.Save();

            response.Result = result;
            response.Success = result;
            response.ResponsCode = result ? 200 : 400;
            response.Message = result ? "User deleted successfully" : "Failed to delete user";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred while deleting the user";
        }

        return response;
    }

    public async Task<HttpResponseData<bool>> ToggleUserStatusAsync(int id)
    {
        var response = new HttpResponseData<bool>();

        try
        {
            var user = await _userRepository.GetUser(id);
            if (user == null)
            {
                response.Success = false;
                response.ResponsCode = 404;
                response.Error = "User not found";
                response.Message = "The specified user does not exist";
                return response;
            }

            user.Active = !user.Active;
            _userRepository.Update(user);
            var result = await _userRepository.Save();

            response.Result = result;
            response.Success = result;
            response.ResponsCode = result ? 200 : 400;
            response.Message = result ? $"User status toggled to {(user.Active ? "Active" : "Inactive")} successfully" : "Failed to toggle user status";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred while toggling user status";
        }

        return response;
    }

    public async Task<HttpResponseData<UserDto>> GetLoggedInUserProfileAsync(int userId)
    {
        var response = new HttpResponseData<UserDto>();

        try
        {
            var user = await _userRepository.GetUserProfileAsync(userId);

            response.Result = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                EPF_No = user.EPF_No,
                RoleId = user.RoleId,
                RoleName = user.Role?.RoleName ?? string.Empty,
                Active = user.Active,
                InitialAttempt = user.InitialAttempt,
                CreatedUser = user.CreatedUser,
                CreatedDate = user.CreatedDate,
                Phone = user.PhoneNo
            };

            response.Success = true;
            response.ResponsCode = 200;
            response.Message = "Profile retrieved successfully";
        }
        catch (KeyNotFoundException ex)
        {
            response.Success = false;
            response.ResponsCode = 404;
            response.Error = ex.Message;
            response.Message = "User not found";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred while retrieving profile";
        }

        return response;
    }

    public async Task<HttpResponseData<bool>> ChangePasswordAsync(int userId, ChangePassword dto)
    {
        var response = new HttpResponseData<bool>();

        try
        {
            var user = await _userRepository.GetUser(userId);

            // Validate current password
            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password))
            {
                response.Success = false;
                response.ResponsCode = 400;
                response.Message = "Current password is incorrect";
                return response;
            }

            // Prevent reusing same password
            if (BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.Password))
            {
                response.Success = false;
                response.ResponsCode = 400;
                response.Message = "New password cannot be same as current password";
                return response;
            }

            // Hash new password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            user.Password = hashedPassword;

            _userRepository.Update(user);
            var result = await _userRepository.Save();

            response.Result = result;
            response.Success = result;
            response.ResponsCode = result ? 200 : 400;
            response.Message = result ? "Password changed successfully" : "Failed to change password";
        }
        catch (KeyNotFoundException)
        {
            response.Success = false;
            response.ResponsCode = 404;
            response.Message = "User not found";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred while changing password";
        }

        return response;
    }

    public async Task<HttpResponseData<bool>> LogoutAsync(string token)
    {
        var response = new HttpResponseData<bool>();

        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                response.Success = false;
                response.ResponsCode = 400;
                response.Message = "Token is required";
                return response;
            }

            await _userRepository.RevokeTokenAsync(token);

            response.Result = true;
            response.Success = true;
            response.ResponsCode = 200;
            response.Message = "Logged out successfully";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ResponsCode = 500;
            response.Error = ex.Message;
            response.Message = "An error occurred during logout";
        }

        return response;
    }

}



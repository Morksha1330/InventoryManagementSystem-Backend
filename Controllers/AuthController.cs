using BCrypt.Net;
using InventoryMgtSystem.Data;
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Security.Claims;
using static BCrypt.Net.BCrypt;
using static System.Net.WebRequestMethods;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using InventoryMgtSystem.Handlers;



namespace InventoryMgtSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private static Random random = new Random();
        private readonly IUserRepository _userservice;
        private readonly IJwtService _jwt;

        public AuthController(ApplicationDbContext database, IUserRepository userservice, IJwtService jwtService)
        {
            _database = database;
            _userservice = userservice;
            _jwt = jwtService;
        }
        // Update your AuthController Login method
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginDto login)
        {
            var response = new HttpResponseData<object>();
            try
            {
                var user = await _database.Users
                    .Include(u => u.Role) 
                    .FirstOrDefaultAsync(u => u.Username == login.Username || u.Email == login.Username);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid credentials";
                    response.ResponsCode = 401;
                    return Unauthorized(response);
                }

                var result = BCrypt.Net.BCrypt.Verify(login.Password, user.Password); 
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Invalid credentials";
                    response.ResponsCode = 401;
                    return Unauthorized(response);
                }

                var token = _jwt.CreateToken(user);

                response.Success = true;
                response.Message = "Login successful";
                response.Result = new
                {
                    token,
                    user = new
                    {
                        user.Id,
                        user.Username,
                        user.Name,
                        user.Email,
                        RoleName = user.Role.RoleName ?? "User", 
                        user.EPF_No,
                        user.Active
                    }
                };
                response.ResponsCode = 200;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to login";
                response.Error = ex.Message;
                response.ResponsCode = 500;
                return StatusCode(500, response);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto register) 
        {
            var response = new HttpResponseData<object>();

            var result = _database.Users.FirstOrDefault(x => x.Username == register.Username);
            var length = 7;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var otp = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            var user = new User();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(otp, workFactor: 13);

            if (result == null)
            {
                user.Name = register.Name;
                user.EPF_No = register.EPF_No;
                user.Username = register.Username;
                user.Email = register.Email;
                user.RoleId = register.Role=="Admin"? 1:0;
                user.Active = true;
                user.InitialAttempt = 0;
                user.Password = hashedPassword;

                                //Convert.ToString(otp);

                _database.Add(user);
                _database.SaveChanges();


                var emailService = new EmailService();

                var subject = "Your Account Password";
                var body = $"<h3>Welcome {user.Name}</h3>" +
                           $"<p>Your temporary password is: <b>{otp}</b></p>" +
                           "<p>Please change your password after login.</p>";

                emailService.SendEmail(user.Email, subject, body);

                response.Success = true;
                response.Message = "User registered successfully. Password sent to email.";

                response.Success = true;
                response.Message = "User Registration Success!!";
                response.ResponsCode = 200;

                return Ok(response);

            }
            return Ok(500);
        }

        [HttpPost("change")]
        public IActionResult ChangePassword([FromBody] ChangePassword login)
        {
            var response = new HttpResponseData<object>();

            var userId = Convert.ToInt32(User.FindFirst("id")?.Value);

            var user = _database.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                response.ResponsCode = 404;
                return NotFound(response);
            }

            var passwordCheck = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

            if (!passwordCheck)
            {
                response.Success = false;
                response.Message = "Current password is incorrect";
                response.ResponsCode = 401;
                return Unauthorized(response);
            }

            if (login.newPassword != login.ConfirmPassword)
            {
                response.Success = false;
                response.Message = "New passwords do not match";
                response.ResponsCode = 400;
                return BadRequest(response);
            }

            // Update password correctly
            user.Password = BCrypt.Net.BCrypt.HashPassword(login.newPassword, workFactor: 13);
            user.InitialAttempt = 1;

            _database.SaveChanges();

            response.Success = true;
            response.Message = "Password changed successfully";
            response.ResponsCode = 200;

            return Ok(response);
        }



    }
}

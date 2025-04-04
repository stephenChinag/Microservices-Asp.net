// UserService/Controllers/UsersController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService.Services.UserService _userService;

        public UsersController(UserService.Services.UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);
            if (result == null)
            {
                return BadRequest(new { message = "Username or email already exists" });
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var (user, token) = await _userService.AuthenticateAsync(loginDto);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new { user, token });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserRegisterDto updateDto)
        {
            var result = await _userService.UpdateUserAsync(id, updateDto);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "User updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "User deleted successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/set-admin")]
        public async Task<IActionResult> SetAdminRole(int id, [FromBody] bool isAdmin)
        {
            var result = await _userService.SetAdminRoleAsync(id, isAdmin);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = $"User admin status set to {isAdmin}" });
        }
    }
}

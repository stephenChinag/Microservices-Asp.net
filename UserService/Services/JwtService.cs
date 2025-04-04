// UserService/Services/UserService.cs
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Shared.Interfaces;
using Shared.Models;
using UserService.Data;
using UserService.Models;

namespace UserService.Services
{
    public class UserService
    {
        private readonly UserDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IEventBus _eventBus;

        public UserService(UserDbContext context, JwtService jwtService, IEventBus eventBus)
        {
            _context = context;
            _jwtService = jwtService;
            _eventBus = eventBus;
        }

        public async Task<(UserDto user, string token)> AuthenticateAsync(UserLoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return (null, null);
            }

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generate token
            var token = _jwtService.GenerateJwtToken(user);

            return (MapToDto(user), token);
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto registerDto)
        {
            // Check if username or email already exists
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
            {
                return null;
            }

            var newUser = new User
            {
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Email = registerDto.Email,
                IsAdmin = false, // Regular users by default
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Publish integration event
            _eventBus.Publish(new UserCreatedIntegrationEvent(
                newUser.Id,
                newUser.Username,
                newUser.Email
            ));

            return MapToDto(newUser);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<bool> UpdateUserAsync(int id, UserRegisterDto updateDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            // Update properties if provided
            if (!string.IsNullOrEmpty(updateDto.Username))
            {
                user.Username = updateDto.Username;
            }

            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                user.Email = updateDto.Email;
            }

            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetAdminRoleAsync(int id, bool isAdmin)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            user.IsAdmin = isAdmin;
            await _context.SaveChangesAsync();
            return true;
        }

        // Helper method to map User entity to UserDto
        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };
        }
    }
}

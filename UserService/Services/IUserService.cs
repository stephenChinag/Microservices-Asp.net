// Step 9: Create User Service Interface (UserService/Services/IUserService.cs)
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Models.DTOs;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> UpdateUserAsync(Guid id, UserDto userDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}

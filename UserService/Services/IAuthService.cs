using System.Threading.Tasks;
using UserService.Models;
using UserService.Models.DTOs;

namespace UserService.Services
{
    public interface IAuthService
    {
        Task<(User User, string Token)> RegisterAsync(RegisterDto registerDto);
        Task<(User User, string Token)> LoginAsync(LoginDto loginDto);
    }
}

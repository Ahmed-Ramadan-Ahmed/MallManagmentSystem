using System.Threading.Tasks;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IAuthService
    {
        Task<(bool success, string token, string refreshToken)> LoginAsync(string username, string password, string ipAddress);
        Task<(bool success, string token, string refreshToken)> RefreshTokenAsync(string token, string refreshToken, string ipAddress);
        Task<bool> RevokeTokenAsync(string token, string ipAddress);
        Task<bool> RegisterAsync(User user, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ValidateTokenAsync(string token);
    }
}
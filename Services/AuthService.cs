using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly MallDBContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(MallDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(bool success, string token, string refreshToken)> LoginAsync(string username, string password, string ipAddress)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
                return (false, null, null);

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generate tokens
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user, ipAddress);

            return (true, token, refreshToken.Token);
        }

        public async Task<(bool success, string token, string refreshToken)> RefreshTokenAsync(string token, string refreshToken, string ipAddress)
        {
            var user = await GetUserFromToken(token);
            if (user == null)
                return (false, null, null);

            var storedRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == user.Id);

            if (storedRefreshToken == null || storedRefreshToken.IsRevoked || storedRefreshToken.ExpiryDate <= DateTime.UtcNow)
                return (false, null, null);

            // Generate new tokens
            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user, ipAddress);

            // Revoke old refresh token
            storedRefreshToken.IsRevoked = true;
            storedRefreshToken.RevokedAt = DateTime.UtcNow;
            storedRefreshToken.RevokedByIp = ipAddress;
            storedRefreshToken.ReplacedByToken = newRefreshToken.Token;

            await _context.SaveChangesAsync();

            return (true, newToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeTokenAsync(string token, string ipAddress)
        {
            var user = await GetUserFromToken(token);
            if (user == null)
                return false;

            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == user.Id && !rt.IsRevoked);

            if (refreshToken == null)
                return false;

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReasonRevoked = "Revoked by user";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
                return false;

            user.PasswordHash = HashPassword(password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !VerifyPasswordHash(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            // Generate a random password
            var newPassword = GenerateRandomPassword();
            user.PasswordHash = HashPassword(newPassword);

            // TODO: Send email with new password
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var user = await GetUserFromToken(token);
            return user != null;
        }

        #region Helper Methods

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(User user, string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiryDate = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpiryInDays"])),
                CreatedByIp = ipAddress,
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshToken);
            return refreshToken;
        }

        private async Task<User> GetUserFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                return await _context.Users.FindAsync(userId);
            }
            catch
            {
                return null;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return storedHash == hashOfInput;
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new Random();
            var password = new char[12];

            for (int i = 0; i < password.Length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }

        #endregion
    }
} 
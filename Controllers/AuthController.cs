using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var (success, token, refreshToken) = await _authService.LoginAsync(
                    request.Username,
                    request.Password,
                    Request.HttpContext.Connection.RemoteIpAddress?.ToString());

                if (!success)
                    return Unauthorized(new { message = "Invalid username or password" });

                return Ok(new { token, refreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var (success, token, refreshToken) = await _authService.RefreshTokenAsync(
                    request.Token,
                    request.RefreshToken,
                    Request.HttpContext.Connection.RemoteIpAddress?.ToString());

                if (!success)
                    return Unauthorized(new { message = "Invalid token" });

                return Ok(new { token, refreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            try
            {
                var success = await _authService.RevokeTokenAsync(
                    request.Token,
                    Request.HttpContext.Connection.RemoteIpAddress?.ToString());

                if (!success)
                    return BadRequest(new { message = "Invalid token" });

                return Ok(new { message = "Token revoked successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    Role = request.Role
                };

                var success = await _authService.RegisterAsync(user, request.Password);

                if (!success)
                    return BadRequest(new { message = "Username or email already exists" });

                return Ok(new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value);
                var success = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);

                if (!success)
                    return BadRequest(new { message = "Invalid current password" });

                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var success = await _authService.ResetPasswordAsync(request.Email);

                if (!success)
                    return BadRequest(new { message = "Email not found" });

                return Ok(new { message = "Password reset email sent" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RevokeTokenRequest
    {
        public string Token { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
    }
} 
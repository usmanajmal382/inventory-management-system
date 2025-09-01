using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Models;
using inventory_management_system;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyApp.Api.Controllers
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

        [HttpPost("register")]
        [RequirePermission("users.create")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response == null)
                return BadRequest(new { Message = "User already exists." });

            return Ok(new
            {
                Message = "User registered successfully",
                Token = response.Token,
                Expiration = response.Expiration,
                User = new
                {
                    Id = response.UserId,
                    FullName = response.FullName,
                    Email = response.Email,
                    Role = response.Role.ToString(),
                    Permissions = response.Permissions
                }
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
                return Unauthorized(new { Message = "Invalid credentials" });

            return Ok(new
            {
                Message = "Login successful",
                Token = response.Token,
                Expiration = response.Expiration,
                User = new
                {
                    Id = response.UserId,
                    FullName = response.FullName,
                    Email = response.Email,
                    Role = response.Role.ToString(),
                    Permissions = response.Permissions
                }
            });
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst("role")?.Value;

            UserRole role = Enum.TryParse<UserRole>(userRole, out var parsedRole) ? parsedRole : UserRole.User;
            var permissions = UserPermissions.RolePermissions.ContainsKey(role)
                ? UserPermissions.RolePermissions[role]
                : Array.Empty<string>();

            return Ok(new
            {
                UserId = userId,
                FullName = userName,
                Email = userEmail,
                Role = userRole,
                Permissions = permissions
            });
        }

        [HttpGet("permissions")]
        [Authorize]
        public IActionResult GetPermissions()
        {
            var userRole = User.FindFirst("role")?.Value;
            UserRole role = Enum.TryParse<UserRole>(userRole, out var parsedRole) ? parsedRole : UserRole.User;

            var permissions = UserPermissions.RolePermissions.ContainsKey(role)
                ? UserPermissions.RolePermissions[role]
                : Array.Empty<string>();

            return Ok(new { Permissions = permissions });
        }
    }
}
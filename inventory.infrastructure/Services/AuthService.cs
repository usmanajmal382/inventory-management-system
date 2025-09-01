using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Models;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;

namespace MyApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (exists) return null;

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);
            var expiration = _jwtService.GetTokenExpiration();

            var permissions = UserPermissions.RolePermissions.ContainsKey(user.Role)
                ? UserPermissions.RolePermissions[user.Role]
                : Array.Empty<string>();

            return new AuthResponse
            {
                Token = token,
                Expiration = expiration,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Permissions = permissions
            };
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !user.IsActive) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValid) return null;

            var token = _jwtService.GenerateToken(user);
            var expiration = _jwtService.GetTokenExpiration();

            var permissions = UserPermissions.RolePermissions.ContainsKey(user.Role)
                ? UserPermissions.RolePermissions[user.Role]
                : Array.Empty<string>();

            return new AuthResponse
            {
                Token = token,
                Expiration = expiration,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Permissions = permissions
            };
        }
    }
}
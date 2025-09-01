using FirebaseAdmin.Auth.Hash;
using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace inventory.application.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return user is null ? null : MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync() =>
            (await _userRepo.GetAllAsync()).Select(MapToDto);

        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync() =>
            (await _userRepo.GetActiveUsersAsync()).Select(MapToDto);

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            if (await _userRepo.EmailExistsAsync(dto.Email))
                throw new ArgumentException("Email already exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepo.AddAsync(user);
            return MapToDto(user);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id)
                ?? throw new ArgumentException("User not found");

            if (await _userRepo.EmailExistsAsync(dto.Email, id))
                throw new ArgumentException("Email already exists");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            await _userRepo.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task ActivateUserAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id)
                ?? throw new ArgumentException("User not found");

            user.IsActive = true;
            await _userRepo.UpdateAsync(user);
        }

        public async Task DeactivateUserAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id)
                ?? throw new ArgumentException("User not found");

            user.IsActive = false;
            await _userRepo.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            if (!await _userRepo.ExistsAsync(id))
                throw new ArgumentException("User not found");

            await _userRepo.DeleteAsync(id);
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
        {
            var user = await _userRepo.GetByIdAsync(userId)
                ?? throw new ArgumentException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                throw new ArgumentException("Current password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _userRepo.UpdateAsync(user);
        }

        public async Task<string[]> GetUserPermissionsAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId)
                ?? throw new ArgumentException("User not found");

            return UserPermissions.RolePermissions.ContainsKey(user.Role)
                ? UserPermissions.RolePermissions[user.Role]
                : Array.Empty<string>();
        }

        private static UserDto MapToDto(User user) => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}

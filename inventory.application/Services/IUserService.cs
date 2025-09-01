using inventory.application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<UserDto>> GetActiveUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task ActivateUserAsync(Guid id);
        Task DeactivateUserAsync(Guid id);
        Task DeleteUserAsync(Guid id);
        Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
        Task<string[]> GetUserPermissionsAsync(Guid userId);
    }
}

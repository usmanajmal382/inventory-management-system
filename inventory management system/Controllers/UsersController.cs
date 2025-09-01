using inventory.application.DTOs;
using inventory.application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [RequirePermission("users.read")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get() =>
            Ok(await _userService.GetAllUsersAsync());

        [HttpGet("active")]
        [RequirePermission("users.read")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetActive() =>
            Ok(await _userService.GetActiveUsersAsync());

        [HttpGet("{id:guid}")]
        [RequirePermission("users.read")]
        public async Task<ActionResult<UserDto>> Get(Guid id) =>
            await _userService.GetUserByIdAsync(id) is { } user ? Ok(user) : NotFound();

        [HttpPost]
        [RequirePermission("users.create")]
        public async Task<ActionResult<UserDto>> Post(CreateUserDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await _userService.CreateUserAsync(dto)).Id }, await _userService.CreateUserAsync(dto));

        [HttpPut("{id:guid}")]
        [RequirePermission("users.update")]
        public async Task<ActionResult<UserDto>> Put(Guid id, UpdateUserDto dto) =>
            Ok(await _userService.UpdateUserAsync(id, dto));

        [HttpPatch("{id:guid}/activate")]
        [RequirePermission("users.update")]
        public async Task<IActionResult> Activate(Guid id)
        {
            await _userService.ActivateUserAsync(id);
            return NoContent();
        }

        [HttpPatch("{id:guid}/deactivate")]
        [RequirePermission("users.update")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            await _userService.DeactivateUserAsync(id);
            return NoContent();
        }

        [HttpPost("{id:guid}/change-password")]
        [RequirePermission("users.update")]
        public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordDto dto)
        {
            await _userService.ChangePasswordAsync(id, dto);
            return NoContent();
        }

        [HttpGet("{id:guid}/permissions")]
        [RequirePermission("users.read")]
        public async Task<ActionResult<string[]>> GetPermissions(Guid id) =>
            Ok(await _userService.GetUserPermissionsAsync(id));

        [HttpDelete("{id:guid}")]
        [RequirePermission("users.delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}

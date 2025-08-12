using inventory.application.DTOs;
using inventory.application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApp.Api.Controllers
{
}
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
                Email = response.Email
            }
        });
    }

    [HttpPost("login")]
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
                Email = response.Email
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

        return Ok(new
        {
            UserId = userId,
            FullName = userName,
            Email = userEmail
        });
    }
}


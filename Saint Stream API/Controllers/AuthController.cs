using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Saint_Stream_API.DTOs.Auth;
using Saint_Stream_API.Services;

namespace Saint_Stream_API.Controllers;

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
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        try
        {
            await _authService.RegisterAsync(registerDto);

            return Ok(new
            {
                message = "Registration successful."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            AuthResponseDto response = await _authService.LoginAsync(loginDto);

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            AuthResponseDto response = await _authService.RefreshTokenAsync(refreshTokenDto);

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            await _authService.LogoutAsync(refreshTokenDto);

            return Ok(new { message = "Logout successful." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return Unauthorized();

        return Ok(new
        {
            id = userId,
            username = User.FindFirstValue(ClaimTypes.Name)
        });
    }
}

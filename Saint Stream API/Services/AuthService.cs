using System.Security.Cryptography;
using Saint_Stream_API.Data;
using Saint_Stream_API.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saint_Stream_API.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Saint_Stream_API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(
        AppDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        bool emailExists = await _context.Users
            .AnyAsync(u => u.Email == registerDto.Email);

        if (emailExists)
            throw new InvalidOperationException("Email already exists.");

        bool usernameExists = await _context.Users
            .AnyAsync(u => u.Username == registerDto.Username);

        if (usernameExists)
            throw new InvalidOperationException("Username already exists.");

        User user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var passwordHasher = new PasswordHasher<User>();

        user.PasswordHash = passwordHasher.HashPassword(
            user,
            registerDto.Password
        );

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }


    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is inactive.");

        var passwordHasher = new PasswordHasher<User>();

        var result = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            loginDto.Password
        );

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid email or password.");

        string accessToken = GenerateToken(user);
        string refreshToken = GenerateRefreshToken();

        RefreshToken refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await _context.RefreshTokens.AddAsync(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        RefreshToken? existingToken = await _context.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == refreshTokenDto.RefreshToken);

        if (existingToken is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        if (existingToken.ExpiresAt <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token has expired.");

        User user = existingToken.User;

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is inactive.");

        _context.RefreshTokens.Remove(existingToken);

        string newAccessToken = GenerateToken(user);
        string newRefreshToken = GenerateRefreshToken();

        RefreshToken newToken = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await _context.RefreshTokens.AddAsync(newToken);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
    public async Task LogoutAsync(RefreshTokenDto refreshTokenDto)
    {
        RefreshToken? refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshTokenDto.RefreshToken);

        if (refreshToken is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        _context.RefreshTokens.Remove(refreshToken);

        await _context.SaveChangesAsync();
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private string GenerateRefreshToken()
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }
}

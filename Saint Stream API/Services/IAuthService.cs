using Saint_Stream_API.DTOs.Auth;

namespace Saint_Stream_API.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    Task LogoutAsync(RefreshTokenDto refreshTokenDto);

}
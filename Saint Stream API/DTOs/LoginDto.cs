using System.ComponentModel.DataAnnotations;

namespace Saint_Stream_API.DTOs.Auth;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
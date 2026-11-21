using System.ComponentModel.DataAnnotations;

namespace Saint_Stream_API.DTOs.Auth;

public class RegisterDto
{
    [Required]
    [MaxLength(20)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}
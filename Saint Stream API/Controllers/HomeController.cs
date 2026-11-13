
using Microsoft.AspNetCore.Mvc;
using Saint_Stream_API.DTOs;
using Saint_Stream_API.Models;
using Saint_Stream_API.Services;

namespace Saint_Stream_API.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;

    private static readonly string[] AllowedSections =
    {
        "just-release",
        "popular-week",
        "movies",
        "series",
        "korean-series",
        "awards",
        "fast",
        "live"
    };

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet("hero")]
    public async Task<IActionResult> GetHero()
    {
        var hero = await _homeService.GetHeroAsync();

        return Ok(new ApiResponse<List<HeroDto>>
        {
            Success = true,
            Message = "Hero items retrieved successfully",
            Data = hero
        });
    }

    [HttpGet("platforms")]
    public async Task<IActionResult> GetPlatforms()
    {
        var platforms = await _homeService.GetPlatformsAsync();

        return Ok(new ApiResponse<List<PlatformDto>>
        {
            Success = true,
            Message = "Platforms retrieved successfully",
            Data = platforms
        });
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured()
    {
        var featured = await _homeService.GetFeaturedAsync();

        return Ok(new ApiResponse<List<FeaturedDto>>
        {
            Success = true,
            Message = "Featured items retrieved successfully",
            Data = featured
        });
    }

    [HttpGet("sections")]
    public async Task<IActionResult> GetSections(
        [FromQuery] string? section,
        [FromQuery] string? limit)
    {
        if (string.IsNullOrWhiteSpace(section) ||
            !AllowedSections.Contains(section))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = $"Invalid section. Allowed values: {string.Join(", ", AllowedSections)}",
                Data = null
            });
        }

        var parsedLimit = 10;

        if (limit != null)
        {
            if (!int.TryParse(limit, out parsedLimit) ||
                parsedLimit < 1 ||
                parsedLimit > 20)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Limit must be a number between 1 and 20",
                    Data = null
                });
            }
        }

        var result = await _homeService.GetSectionsAsync(
            section,
            parsedLimit);

        return Ok(new ApiResponse<SectionResponseDto>
        {
            Success = true,
            Message = "Section retrieved successfully",
            Data = result
        });
    }
}

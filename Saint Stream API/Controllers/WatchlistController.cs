using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saint_Stream_API.DTOs.Watchlist;
using Saint_Stream_API.Services;

namespace Saint_Stream_API.Controllers;

[Route("api/watchlist")]
[ApiController]
[Authorize]
public class WatchlistController : ControllerBase
{
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(IWatchlistService watchlistService)
    {
        _watchlistService = watchlistService;
    }

    private int GetUserId()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userId, out int id))
            throw new UnauthorizedAccessException();

        return id;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddToWatchlistDto dto)
    {
        try
        {
            var item = await _watchlistService.AddAsync(GetUserId(), dto);

            return StatusCode(201, new
            {
                id = item.Id,
                content_id = item.ContentId,
                added_at = item.AddedAt
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{contentId:int}")]
    public async Task<IActionResult> Remove(int contentId)
    {
        if (contentId <= 0)
            return BadRequest(new { message = "Invalid contentId" });

        try
        {
            await _watchlistService.RemoveAsync(GetUserId(), contentId);

            return Ok(new
            {
                message = "Removed from watchlist"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _watchlistService.GetAllAsync(GetUserId());

        return Ok(result);
    }
}
using System.ComponentModel.DataAnnotations;

namespace Saint_Stream_API.DTOs.Watchlist;

public class AddToWatchlistDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int? ContentId { get; set; }
}
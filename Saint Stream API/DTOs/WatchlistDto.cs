namespace Saint_Stream_API.DTOs.Watchlist;

public class WatchlistDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? PosterUrl { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal? Rating { get; set; }
    public string? AgeRating { get; set; }
    public List<string> Genres { get; set; } = new();
    public DateTime AddedAt { get; set; }
}
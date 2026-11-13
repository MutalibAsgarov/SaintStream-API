namespace Saint_Stream_API.DTOs;

public class FeaturedDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Tagline { get; set; }
    public decimal? Rating { get; set; }
    public int? DurationMinutes { get; set; }
    public short ReleaseYear { get; set; }
    public List<string> Genres { get; set; } = new();
    public string? Description { get; set; }
    public string? PosterUrl { get; set; }
    public string? BackdropUrl { get; set; }
}
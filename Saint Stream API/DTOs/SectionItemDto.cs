namespace Saint_Stream_API.DTOs;

public class SectionItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? PosterUrl { get; set; }
    public string Type { get; set; } = null!;
    public decimal? Rating { get; set; }
    public string? AgeRating { get; set; }
    public short ReleaseYear { get; set; }
    public List<string> Genres { get; set; } = new();
    public int? SeasonCount { get; set; }

    public int? Rank { get; set; }

    public string? Label { get; set; }
    public string? Description { get; set; }
    public string? BackdropUrl { get; set; }
    public int? DurationMinutes { get; set; }
    public PlatformInfoDto? Platform { get; set; }
}
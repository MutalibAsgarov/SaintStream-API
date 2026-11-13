namespace Saint_Stream_API.DTOs;

public class HeroDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public short ReleaseYear { get; set; }
    public int? DurationMinutes { get; set; }
    public int? SeasonCount { get; set; }
    public List<string> Genres { get; set; } = new();
    public string? Description { get; set; }
    public string? BackdropUrl { get; set; }
    public string? TrailerUrl { get; set; }
}
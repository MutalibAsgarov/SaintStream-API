
namespace Saint_Stream_API.Entities;

public class Content
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? PosterUrl { get; set; }
    public string? BackdropUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public string Type { get; set; } = null!;
    public string? OriginCountry { get; set; }
    public short ReleaseYear { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public int? DurationMinutes { get; set; }
    public int? SeasonCount { get; set; }
    public decimal? Rating { get; set; }
    public string? AgeRating { get; set; }
    public ICollection<ContentGenre> ContentGenres { get; set; } = new List<ContentGenre>();
}

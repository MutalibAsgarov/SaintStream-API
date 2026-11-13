namespace Saint_Stream_API.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;

    public ICollection<ContentGenre> ContentGenres { get; set; } = new List<ContentGenre>();
}
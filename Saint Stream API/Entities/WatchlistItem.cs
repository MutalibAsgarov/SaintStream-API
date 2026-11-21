namespace Saint_Stream_API.Entities;

public class WatchlistItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public DateTime AddedAt { get; set; }

    public User User { get; set; } = null!;
    public Content Content { get; set; } = null!;
}
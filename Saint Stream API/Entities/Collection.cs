namespace Saint_Stream_API.Entities;

public class Collection
{
    public int Id { get; set; }
    public string Slug { get; set; } = null!;
    public string Title { get; set; } = null!;

    public ICollection<CollectionItem> CollectionItems { get; set; } = new List<CollectionItem>();
}
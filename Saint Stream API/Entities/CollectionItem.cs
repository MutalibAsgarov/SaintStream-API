namespace Saint_Stream_API.Entities;

public class CollectionItem
{
    public int Id { get; set; }
    public int CollectionId { get; set; }
    public int ContentId { get; set; }
    public int OrderNumber { get; set; }
    public string? Label { get; set; }

    public Collection Collection { get; set; } = null!;
    public Content Content { get; set; } = null!;
}
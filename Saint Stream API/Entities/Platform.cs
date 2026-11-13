namespace Saint_Stream_API.Entities;

public class Platform
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string LogoUrl { get; set; } = null!;
    public int OrderNumber { get; set; }
    public bool IsActive { get; set; }
}
namespace Saint_Stream_API.DTOs;

public class SectionResponseDto
{
    public string Section { get; set; } = null!;
    public List<SectionItemDto> Items { get; set; } = new();
}
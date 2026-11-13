
using Saint_Stream_API.DTOs;

namespace Saint_Stream_API.Services;

public interface IHomeService
{
    Task<List<HeroDto>> GetHeroAsync();

    Task<List<PlatformDto>> GetPlatformsAsync();

    Task<List<FeaturedDto>> GetFeaturedAsync();

    Task<SectionResponseDto> GetSectionsAsync(
        string section,
        int limit);
}

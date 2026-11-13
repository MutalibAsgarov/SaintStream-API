using Microsoft.EntityFrameworkCore;
using Saint_Stream_API.Data;
using Saint_Stream_API.DTOs;

namespace Saint_Stream_API.Services;

public class HomeService : IHomeService
{
    private readonly AppDbContext _context;

    public HomeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HeroDto>> GetHeroAsync()
    {
        var contents = await _context.CollectionItems
            .Where(x => x.Collection.Slug == "hero")
            .OrderBy(x => x.OrderNumber)
            .Take(5)
            .Select(x => x.Content)
            .ToListAsync();

        return contents.Select(x => new HeroDto
        {
            Id = x.Id,
            Title = x.Title,
            Type = x.Type,
            ReleaseYear = x.ReleaseYear,
            DurationMinutes = x.DurationMinutes,
            SeasonCount = x.SeasonCount,
            Description = x.Description,
            BackdropUrl = x.BackdropUrl,
            TrailerUrl = x.TrailerUrl,
            Genres = x.ContentGenres.Select(g => g.Genre.Name).ToList()
        }).ToList();
    }

    public async Task<List<PlatformDto>> GetPlatformsAsync()
    {
        return await _context.Platforms
            .Where(x => x.IsActive)
            .OrderBy(x => x.OrderNumber)
            .Select(x => new PlatformDto
            {
                Id = x.Id,
                Name = x.Name,
                LogoUrl = x.LogoUrl
            })
            .ToListAsync();
    }

    public async Task<List<FeaturedDto>> GetFeaturedAsync()
    {
        var items = await _context.CollectionItems
            .Where(x => x.Collection.Slug == "featured")
            .OrderBy(x => x.OrderNumber)
            .Select(x => new FeaturedDto
            {
                Id = x.Content.Id,
                Title = x.Content.Title,
                Tagline = x.Label,
                Rating = x.Content.Rating,
                DurationMinutes = x.Content.DurationMinutes,
                ReleaseYear = x.Content.ReleaseYear,
                Description = x.Content.Description,
                PosterUrl = x.Content.PosterUrl,
                BackdropUrl = x.Content.BackdropUrl,
                Genres = x.Content.ContentGenres
                    .Select(g => g.Genre.Name).ToList()
            })
            .ToListAsync();

        return items;
    }

    public async Task<SectionResponseDto> GetSectionsAsync(string section, int limit)
    {
        var contents = await _context.Contents
            .Where(x => section == "movies" ? x.Type == "movie" :
                        section == "series" ? x.Type == "series" :
                        section == "korean-series" ? x.Type == "series" && x.OriginCountry == "KR" :
                        true)
            .OrderByDescending(x => x.ReleaseDate)
            .Take(limit)
            .Select(x => new SectionItemDto
            {
                Id = x.Id,
                Title = x.Title,
                PosterUrl = x.PosterUrl,
                Type = x.Type,
                Rating = x.Rating,
                AgeRating = x.AgeRating,
                ReleaseYear = x.ReleaseYear,
                SeasonCount = x.SeasonCount,
                Genres = x.ContentGenres.Select(g => g.Genre.Name).ToList()
            })
            .ToListAsync();

        return new SectionResponseDto
        {
            Section = section,
            Items = contents
        };
    }
}
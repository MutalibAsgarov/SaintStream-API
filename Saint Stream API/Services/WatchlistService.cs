using Microsoft.EntityFrameworkCore;
using Saint_Stream_API.Data;
using Saint_Stream_API.DTOs.Watchlist;
using Saint_Stream_API.Entities;

namespace Saint_Stream_API.Services;

public class WatchlistService : IWatchlistService
{
    private readonly AppDbContext _context;

    public WatchlistService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WatchlistItem> AddAsync(int userId, AddToWatchlistDto dto)
    {
        int contentId = dto.ContentId!.Value;

        bool contentExists = await _context.Contents
            .AnyAsync(c => c.Id == contentId);

        if (!contentExists)
            throw new KeyNotFoundException("Content not found");

        bool alreadyExists = await _context.WatchlistItems
            .AnyAsync(w => w.UserId == userId && w.ContentId == contentId);

        if (alreadyExists)
            throw new InvalidOperationException("Already in watchlist");

        WatchlistItem item = new WatchlistItem
        {
            UserId = userId,
            ContentId = contentId,
            AddedAt = DateTime.UtcNow
        };

        await _context.WatchlistItems.AddAsync(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task RemoveAsync(int userId, int contentId)
    {
        WatchlistItem? item = await _context.WatchlistItems
            .FirstOrDefaultAsync(w =>
                w.UserId == userId &&
                w.ContentId == contentId);

        if (item is null)
            throw new KeyNotFoundException("Not in watchlist");

        _context.WatchlistItems.Remove(item);

        await _context.SaveChangesAsync();
    }

    public async Task<List<WatchlistDto>> GetAllAsync(int userId)
    {
        return await _context.WatchlistItems
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.AddedAt)
            .Select(w => new WatchlistDto
            {
                Id = w.Id,
                Title = w.Content.Title,
                PosterUrl = w.Content.PosterUrl,
                Type = w.Content.Type,
                Rating = w.Content.Rating,
                AgeRating = w.Content.AgeRating,
                Genres = w.Content.ContentGenres
                    .Select(cg => cg.Genre.Name)
                    .ToList(),
                AddedAt = w.AddedAt
            })
            .ToListAsync();
    }
}
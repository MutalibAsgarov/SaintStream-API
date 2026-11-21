using Saint_Stream_API.DTOs.Watchlist;
using Saint_Stream_API.Entities;

namespace Saint_Stream_API.Services;

public interface IWatchlistService
{
    Task<WatchlistItem> AddAsync(int userId, AddToWatchlistDto dto);
    Task RemoveAsync(int userId, int contentId);
    Task<List<WatchlistDto>> GetAllAsync(int userId);
}
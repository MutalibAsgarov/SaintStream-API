
using Microsoft.EntityFrameworkCore;
using Saint_Stream_API.Entities;

namespace Saint_Stream_API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Content> Contents { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<ContentGenre> ContentGenres { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionItem> CollectionItems { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<WatchlistItem> WatchlistItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Content>(entity =>
        {
            entity.ToTable("contents");

            entity.Property(e => e.PosterUrl).HasColumnName("poster_url");
            entity.Property(e => e.BackdropUrl).HasColumnName("backdrop_url");
            entity.Property(e => e.TrailerUrl).HasColumnName("trailer_url");
            entity.Property(e => e.OriginCountry).HasColumnName("origin_country");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.DurationMinutes).HasColumnName("duration_minutes");
            entity.Property(e => e.SeasonCount).HasColumnName("season_count");
            entity.Property(e => e.AgeRating).HasColumnName("age_rating");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("genres");
        });

        modelBuilder.Entity<ContentGenre>(entity =>
        {
            entity.ToTable("content_genres");

            entity.HasKey(e => new { e.ContentId, e.GenreId });

            entity.Property(e => e.ContentId).HasColumnName("content_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");

            entity.HasOne(e => e.Content)
                .WithMany(e => e.ContentGenres)
                .HasForeignKey(e => e.ContentId);

            entity.HasOne(e => e.Genre)
                .WithMany(e => e.ContentGenres)
                .HasForeignKey(e => e.GenreId);
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.ToTable("collections");
        });

        modelBuilder.Entity<CollectionItem>(entity =>
        {
            entity.ToTable("collection_items");

            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.ContentId).HasColumnName("content_id");
            entity.Property(e => e.OrderNumber).HasColumnName("order_number");

            entity.HasOne(e => e.Collection)
                .WithMany(e => e.CollectionItems)
                .HasForeignKey(e => e.CollectionId);

            entity.HasOne(e => e.Content)
                .WithMany()
                .HasForeignKey(e => e.ContentId);
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.ToTable("platforms");

            entity.Property(e => e.LogoUrl).HasColumnName("logo_url");
            entity.Property(e => e.OrderNumber).HasColumnName("order_number");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Username)
                .HasColumnName("username")
                .HasMaxLength(20)
                .IsRequired();

            entity.HasIndex(e => e.Username)
                .IsUnique();

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        });
        modelBuilder.Entity<WatchlistItem>(entity =>
        {
            entity.ToTable("watchlist_items");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.ContentId)
                .HasColumnName("content_id");

            entity.HasIndex(e => new { e.UserId, e.ContentId })
                .IsUnique();

            entity.Property(e => e.AddedAt)
                .HasColumnName("added_at");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Content)
                .WithMany()
                .HasForeignKey(e => e.ContentId);
        });
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.Token)
                .HasColumnName("token")
                .HasMaxLength(500)
                .IsRequired();

            entity.HasIndex(e => e.Token)
                .IsUnique();

            entity.Property(e => e.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("password_reset_tokens");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.Token)
                .HasColumnName("token")
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(e => e.Token)
                .IsUnique();

            entity.Property(e => e.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            entity.Property(e => e.IsUsed)
                .HasColumnName("is_used")
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

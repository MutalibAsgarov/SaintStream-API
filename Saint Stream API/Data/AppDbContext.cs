
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
    }
}


using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saint_Stream_API.Migrations
{
    public partial class AddWatchlistUniqueConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_watchlist_items_user_id_content_id",
                table: "watchlist_items",
                columns: new[] { "user_id", "content_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_watchlist_items_user_id_content_id",
                table: "watchlist_items");
        }
    }
}

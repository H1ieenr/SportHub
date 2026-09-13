using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sporthub.repository.context.migrations
{
    /// <inheritdoc />
    public partial class UpdateCategorySlugIndexFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_categories_slug",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "IX_categories_slug",
                table: "categories",
                column: "slug",
                unique: true,
                filter: "[deleted_date] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_categories_slug",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "IX_categories_slug",
                table: "categories",
                column: "slug",
                unique: true,
                filter: "[is_deleted] = 0");
        }
    }
}
